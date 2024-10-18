using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Exceptions;
using TraffiLearn.Application.Users.Identity;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.Errors;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Auth.Commands.ConfirmEmail
{
    internal sealed class ConfirmEmailCommandHandler
        : IRequestHandler<ConfirmEmailCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IIdentityService<ApplicationUser> _identityService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ConfirmEmailCommandHandler> _logger;

        public ConfirmEmailCommandHandler(
            IUserRepository userRepository,
            IIdentityService<ApplicationUser> identityService,
            IUnitOfWork unitOfWork,
            ILogger<ConfirmEmailCommandHandler> logger)
        {
            _userRepository = userRepository;
            _identityService = identityService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(
            ConfirmEmailCommand request,
            CancellationToken cancellationToken)
        {
            var userId = new UserId(request.UserId.Value);

            var user = await _userRepository.GetByIdAsync(
                userId,
                cancellationToken);

            if (user is null)
            {
                return UserErrors.NotFound;
            }

            if (user.IsEmailConfirmed)
            {
                return UserErrors.EmailAlreadyConfirmed;
            }

            var identityUser = await _identityService.GetByEmailAsync(
                email: user.Email);

            if (identityUser is null)
            {
                _logger.LogCritical("User is found in the repository, but not found in identity storage. Possible data consistency failure.");

                throw new DataInconsistencyException();
            }

            await using var transaction = await _unitOfWork
                .BeginTransactionAsync(
                    cancellationToken: cancellationToken);

            try
            {
                var result = user.ConfirmEmail();

                if (result.IsFailure)
                {
                    return result.Error;
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var identityResult = await _identityService.ConfirmEmailAsync(
                    identityUser,
                    token: request.Token);

                if (identityResult.IsFailure)
                {
                    throw new InvalidOperationException($"Failed to confirm email of an identity user, whereas succeeded to confirm domain user. Error: {result.Error.Description}");
                }

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();

                throw;
            }

            _logger.LogInformation("Succesfully confirmed the email: {email}", user.Email.Value);

            return Result.Success();
        }
    }
}
