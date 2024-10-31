using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Abstractions.Security;
using TraffiLearn.Application.Users.Identity;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.Emails;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Infrastructure.Authentication.Options;

namespace TraffiLearn.Infrastructure.Services
{
    internal sealed class IdentityService : IIdentityService<ApplicationUser>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly LoginSettings _loginSettings;
        private readonly ILogger<IdentityService> _logger;
        private readonly ITokenService _tokenService;
        private readonly IHasher _hasher;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<LoginSettings> loginSettings,
            ILogger<IdentityService> logger,
            ITokenService tokenService,
            IHasher hasher)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _loginSettings = loginSettings.Value;
            _logger = logger;
            _tokenService = tokenService;
            _hasher = hasher;
        }

        public async Task CreateAsync(
            ApplicationUser identityUser,
            string password)
        {
            ArgumentNullException.ThrowIfNull(identityUser, nameof(identityUser));
            ArgumentException.ThrowIfNullOrWhiteSpace(password, nameof(password));

            var result = await _userManager.CreateAsync(identityUser, password);

            HandleIdentityResult(result, "Failed to create identity user.");
        }

        public async Task DeleteAsync(ApplicationUser identityUser)
        {
            ArgumentNullException.ThrowIfNull(identityUser, nameof(identityUser));

            var result = await _userManager.DeleteAsync(identityUser);

            HandleIdentityResult(result, "Failed to delete identity user.");
        }

        public async Task AddToRoleAsync(
            ApplicationUser identityUser,
            string roleName)
        {
            ArgumentNullException.ThrowIfNull(identityUser, nameof(identityUser));
            ArgumentException.ThrowIfNullOrWhiteSpace(roleName, nameof(roleName));

            var result = await _userManager.AddToRoleAsync(identityUser, roleName);

            HandleIdentityResult(result, "Failed to add identity user to role.");
        }

        public async Task RemoveFromRoleAsync(
            ApplicationUser identityUser,
            string roleName)
        {
            ArgumentNullException.ThrowIfNull(identityUser, nameof(identityUser));
            ArgumentException.ThrowIfNullOrWhiteSpace(roleName, nameof(roleName));

            var result = await _userManager.RemoveFromRoleAsync(identityUser, roleName);

            HandleIdentityResult(result, "Failed to remove identity user from role.");
        }

        private void HandleIdentityResult(IdentityResult result, string errorMessage)
        {
            if (result.Succeeded)
            {
                return;
            }

            var errors = result.Errors.Select(x => x.Description);
            var errorsString = string.Join(Environment.NewLine, errors);

            throw new InvalidOperationException($"{errorMessage}\r\nErrors: {errorsString}");
        }

        public async Task<ApplicationUser?> GetByEmailAsync(Email email)
        {
            ArgumentNullException.ThrowIfNull(email, nameof(email));

            return await _userManager.FindByEmailAsync(email.Value);
        }

        public async Task<Result> LoginAsync(
            ApplicationUser identityUser,
            string password)
        {
            ArgumentNullException.ThrowIfNull(identityUser);
            ArgumentException.ThrowIfNullOrWhiteSpace(password);

            var result = await _signInManager.PasswordSignInAsync(
                identityUser,
                password,
                isPersistent: _loginSettings.IsPersistent,
                lockoutOnFailure: _loginSettings.LockoutOnFailure);

            if (!result.Succeeded)
            {
                return UserErrors.InvalidCredentials;
            }

            return Result.Success();
        }

        public async Task<Result> ConfirmEmailAsync(
            ApplicationUser identityUser,
            string token)
        {
            var result = await _userManager.ConfirmEmailAsync(
                identityUser, token);

            if (!result.Succeeded)
            {
                _logger.LogWarning("Failed to confirm email. Potential internal failures. Errors: {errors}", string.Join(", ", result.Errors.Select(e => e.Description)));

                return UserErrors.EmailConfirmationFailure;
            }

            return Result.Success();
        }

        public async Task PopulateRefreshTokenAsync(ApplicationUser user, string refreshToken)
        {
            var refreshTokenHash = _hasher.Hash(refreshToken);

            user.RefreshTokenHash = refreshTokenHash;

            user.RefreshTokenExpirationTime = DateTime.UtcNow.AddDays(
                _loginSettings.RefreshTokenExpiryInDays);

            await _userManager.UpdateAsync(user);
        }

        public Result ValidateRefreshToken(ApplicationUser user)
        {
            if (user.RefreshTokenExpirationTime < DateTime.UtcNow)
            {
                return Result.Failure<ApplicationUser>(UserErrors.RefreshTokenExpired);
            }

            return Result.Success();
        }

        public async Task<Result<ApplicationUser>> GetByRefreshTokenAsync(string refreshToken)
        {
            var refreshTokenHash = _hasher.Hash(refreshToken);

            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.RefreshTokenHash == refreshTokenHash);

            if (user is null)
            {
                return Result.Failure<ApplicationUser>(UserErrors.InvalidRefreshToken);
            }

            return Result.Success(user);
        }

        public async Task<Result> ChangeEmailAsync(
            ApplicationUser identityUser,
            string newEmail,
            string token)
        {
            var identityResult = await _userManager.ChangeEmailAsync(
                user: identityUser,
                newEmail,
                token);

            if (!identityResult.Succeeded)
            {
                return UserErrors.InvalidChangeEmailToken;
            }

            return Result.Success();
        }

        public async Task<Result> ResetPasswordAsync(
            ApplicationUser identityUser,
            string newPassword,
            string token)
        {
            var identityResult = await _userManager.ResetPasswordAsync(
                identityUser,
                token,
                newPassword);

            if (!identityResult.Succeeded)
            {
                return UserErrors.InvalidResetPasswordToken;
            }

            return Result.Success();
        }
    }
}
