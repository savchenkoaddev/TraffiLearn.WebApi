using Microsoft.AspNetCore.Identity;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.Aggregates.Users.Roles;

namespace TraffiLearn.Infrastructure.Helpers
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAsync(IRoleService<IdentityRole> roleManager)
        {
            var roleNames = Enum.GetValues(typeof(Role))
                                .Cast<Role>()
                                .Select(r => r.ToString())
                                .ToArray();

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.ExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}
