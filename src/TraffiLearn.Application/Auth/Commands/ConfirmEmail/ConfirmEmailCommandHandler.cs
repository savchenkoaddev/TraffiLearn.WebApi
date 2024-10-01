using MediatR;
using Microsoft.Extensions.Logging;
using System.Transactions;
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

            var decodedToken = Uri.UnescapeDataString(request.EncodedToken);

            using (var transaction = new TransactionScope(
                TransactionScopeAsyncFlowOption.Enabled))
            {
                var identityResult = await _identityService.ConfirmEmailAsync(
                    identityUser,
                    token: decodedToken);

                if (identityResult.IsFailure)
                {
                    return identityResult.Error;
                }

                var result = user.ConfirmEmail();

                if (result.IsFailure)
                {
                    throw new InvalidOperationException($"Failed to confirm email of a user. Error: {result.Error.Description}");
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                transaction.Complete();
            }

            _logger.LogInformation("Succesfully confirmed the email: {email}", user.Email.Value);

            return Result.Success();
        }
    }
}
