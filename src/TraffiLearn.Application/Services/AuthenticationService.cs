using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Errors;
using TraffiLearn.Application.Options;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Services
{
    internal sealed class AuthenticationService<TIdentityUser>
        : IAuthenticationService<TIdentityUser>
        where TIdentityUser : class
    {
        private readonly UserManager<TIdentityUser> _userManager;
        private readonly SignInManager<TIdentityUser> _signInManager;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly LoginSettings _loginSettings;
        private readonly ILogger<AuthenticationService<TIdentityUser>> _logger;

        public AuthenticationService(
            UserManager<TIdentityUser> userManager,
            SignInManager<TIdentityUser> signInManager,
            IUserRepository userRepository,
            ITokenService tokenService,
            IOptions<LoginSettings> loginSettings,
            ILogger<AuthenticationService<TIdentityUser>> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _loginSettings = loginSettings.Value;
            _logger = logger;
        }

        public async Task<Result<string>> LoginAsync(
            string email,
            string password,
            CancellationToken cancellationToken = default)
        {
            var emailResult = Email.Create(email);

            if (emailResult.IsFailure)
            {
                return Result.Failure<string>(emailResult.Error);
            }

            var identityUser = await _userManager.FindByEmailAsync(email);

            if (identityUser is null)
            {
                return Result.Failure<string>(UserErrors.NotFound);
            }

            var user = await _userRepository.GetByEmailAsync(
                emailResult.Value,
                cancellationToken);

            if (user is null)
            {
                _logger.LogCritical(InternalErrors.DataConsistencyError.Description);

                return Result.Failure<string>(InternalErrors.DataConsistencyError);
            }

            var canLogin = await _signInManager.CanSignInAsync(identityUser);

            if (!canLogin)
            {
                return Result.Failure<string>(UserErrors.CannotLogin);
            }

            var loginResult = await _signInManager.PasswordSignInAsync(
                identityUser,
                password: password,
                isPersistent: _loginSettings.IsPersistent,
                lockoutOnFailure: _loginSettings.LockoutOnFailure);

            if (!loginResult.Succeeded)
            {
                _logger.LogError("Failed to login through SignInManager. IsLockedOut: {isLockedOut}", loginResult.IsLockedOut);

                return Result.Failure<string>(UserErrors.InvalidCredentials);
            }

            var accessToken = _tokenService.GenerateAccessToken(user);

            _logger.LogInformation("Succesfully logged in. Access Token generated.");

            return accessToken;
        }
    }
}
