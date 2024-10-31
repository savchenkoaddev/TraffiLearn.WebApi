using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Exceptions;
using TraffiLearn.Application.Users.Identity;
using TraffiLearn.Domain.Users;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Auth.Commands.RecoverPassword
{
    internal sealed class RecoverPasswordCommandHandler
        : IRequestHandler<RecoverPasswordCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IIdentityService<ApplicationUser> _identityService;
        private readonly ILogger<RecoverPasswordCommandHandler> _logger;

        public RecoverPasswordCommandHandler(
            IUserRepository userRepository,
            IIdentityService<ApplicationUser> identityService,
            ILogger<RecoverPasswordCommandHandler> logger)
        {
            _userRepository = userRepository;
            _identityService = identityService;
            _logger = logger;
        }

        public async Task<Result> Handle(
            RecoverPasswordCommand request,
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
