using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.Aggregates.Users.Errors;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Infrastructure.Authentication.Options;

namespace TraffiLearn.Infrastructure.Services
{
    internal sealed class IdentityService<TIdentityUser> : IIdentityService<TIdentityUser>
        where TIdentityUser : class
    {
        private readonly UserManager<TIdentityUser> _userManager;
        private readonly SignInManager<TIdentityUser> _signInManager;
        private readonly LoginSettings _loginSettings;

        public IdentityService(
            UserManager<TIdentityUser> userManager,
            SignInManager<TIdentityUser> signInManager,
            IOptions<LoginSettings> loginSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _loginSettings = loginSettings.Value;
        }

        public async Task CreateAsync(
            TIdentityUser identityUser,
            string password)
        {
            ArgumentNullException.ThrowIfNull(identityUser, nameof(identityUser));
            ArgumentException.ThrowIfNullOrWhiteSpace(password, nameof(password));

            var result = await _userManager.CreateAsync(identityUser, password);

            HandleIdentityResult(result, "Failed to create identity user.");
        }

        public async Task DeleteAsync(TIdentityUser identityUser)
        {
            ArgumentNullException.ThrowIfNull(identityUser, nameof(identityUser));

            var result = await _userManager.DeleteAsync(identityUser);

            HandleIdentityResult(result, "Failed to delete identity user.");
        }

        public async Task AddToRoleAsync(
            TIdentityUser identityUser,
            string roleName)
        {
            ArgumentNullException.ThrowIfNull(identityUser, nameof(identityUser));
            ArgumentException.ThrowIfNullOrWhiteSpace(roleName, nameof(roleName));

            var result = await _userManager.AddToRoleAsync(identityUser, roleName);


            HandleIdentityResult(result, "Failed to add identity user to role.");
        }

        public async Task RemoveFromRoleAsync(
            TIdentityUser identityUser,
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

        public async Task<TIdentityUser?> GetByEmailAsync(Email email)
        {
            ArgumentNullException.ThrowIfNull(email, nameof(email));

            return await _userManager.FindByEmailAsync(email.Value);
        }

        public async Task<Result> LoginAsync(
            TIdentityUser identityUser,
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
    }
}
