using Microsoft.AspNetCore.Identity;
using TraffiLearn.Application.Abstractions.Identity;

namespace TraffiLearn.Application.Services
{
    public sealed class IdentityRoleManager<TRole> : IRoleManager<TRole>
        where TRole : class
    {
        private readonly RoleManager<TRole> _roleManager;

        public IdentityRoleManager(RoleManager<TRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task CreateRole(TRole role)
        {
            var result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);

                var errorsString = string.Join(Environment.NewLine, errors);

                throw new InvalidOperationException($"Failed to create a new role. Errors: {errorsString}");
            }
        }

        public async Task<bool> RoleExists(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }
    }
}
