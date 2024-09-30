using MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ConfirmEmailCommandHandler> _logger;

        public ConfirmEmailCommandHandler(
            IUserRepository userRepository,
            IIdentityService<ApplicationUser> identityService,
            ILogger<ConfirmEmailCommandHandler> logger)
        {
            _userRepository = userRepository;
            _identityService = identityService;
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

            var result = await _identityService.ConfirmEmailAsync(
                identityUser,
                token: request.Token);

            if (result.IsFailure)
            {
                return result.Error;
            }

            return Result.Success();
        }
    }
}
