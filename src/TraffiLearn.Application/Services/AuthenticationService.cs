using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Errors;
using TraffiLearn.Application.Options;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Services
{
    internal sealed class AuthenticationService : IAuthenticationService<User, Guid>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthenticationService> _logger;

        public AuthenticationService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IUserRepository userRepository,
            ITokenService tokenService,
            ILogger<AuthenticationService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _logger = logger;
        }

        public async Task<Result<User>> GetAuthenticatedUserAsync(
            CancellationToken cancellationToken = default)
        {
            var userIdResult = await GetAuthenticatedUserIdAsync();

            if (userIdResult.IsFailure)
            {
                return Result.Failure<User>(userIdResult.Error);
            }

            UserId userId = new(userIdResult.Value);

            var user = await _userRepository.GetByIdAsync(
                userId,
                cancellationToken);

            if (user is null)
            {
                _logger.LogCritical(InternalErrors.AuthenticatedUserNotFound.Description);

                return Result.Failure<User>(InternalErrors.AuthenticatedUserNotFound);
            }

            return user;
        }

        public async Task<Result<Guid>> GetAuthenticatedUserIdAsync()
        {
            var userClaims = _signInManager.Context.User;

            if (userClaims.Identity is null || !userClaims.Identity.IsAuthenticated)
            {
                _logger.LogError("User is not authenticated. Identity: {Identity}", userClaims.Identity);

                return Result.Failure<Guid>(InternalErrors.AuthorizationFailure);
            }

            var idClaim = userClaims.FindFirst(ClaimTypes.NameIdentifier);

            if (idClaim is null)
            {
                var claimName = "id";
                _logger.LogError("Claim missing. Claim name: {ClaimName}. User Claims: {UserClaims}", claimName, userClaims.Claims);

                return Result.Failure<Guid>(InternalErrors.ClaimMissing(claimName));
            }

            var idString = idClaim.Value;

            if (Guid.TryParse(idString, out Guid id))
            {
                _logger.LogInformation("Successfully parsed user ID. User ID: {UserId}", id);

                return id;
            }

            _logger.LogError("Failed to parse ID to GUID. ID: {IdString}", idString);

            await Task.CompletedTask;

            return Result.Failure<Guid>(Error.InternalFailure());
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
