using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Exceptions;
using TraffiLearn.Application.Users.Identity;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.Errors;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Auth.Commands.ResetPassword
{
    internal sealed class ResetPasswordCommandHandler
        : IRequestHandler<ResetPasswordCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IIdentityService<ApplicationUser> _identityService;
        private readonly ILogger<ResetPasswordCommandHandler> _logger;

        public ResetPasswordCommandHandler(
            IUserRepository userRepository,
            IIdentityService<ApplicationUser> identityService,
            ILogger<ResetPasswordCommandHandler> logger)
        {
            _userRepository = userRepository;
            _identityService = identityService;
            _logger = logger;
        }

        public async Task<Result> Handle(
            ResetPasswordCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(
                new UserId(request.UserId.Value),
                cancellationToken);

            if (user is null)
            {
                return UserErrors.NotFound;
            }

            var identityUser = await _identityService.GetByEmailAsync(
                user.Email);

            if (identityUser is null)
            {
                _logger.LogCritical("User is found in the repository, but not found in identity storage. Possible data consistency failure.");

                throw new DataInconsistencyException();
            }

            var result = await _identityService.ResetPasswordAsync(
                identityUser,
                newPassword: request.NewPassword,
                token: request.Token);

            if (result.IsFailure)
            {
                return result.Error;
            }

            return Result.Success();
        }
    }
}
