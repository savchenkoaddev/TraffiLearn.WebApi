using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Identity;

namespace TraffiLearn.Application.Services
{
    public sealed class IdentityService<TIdentityUser> : IIdentityService<TIdentityUser>
        where TIdentityUser : class
    {
        private readonly UserManager<TIdentityUser> _userManager;

        public IdentityService(UserManager<TIdentityUser> userManager)
        {
            _userManager = userManager;
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
    }
}
