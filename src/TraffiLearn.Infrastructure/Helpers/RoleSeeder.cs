using Microsoft.AspNetCore.Identity;
using TraffiLearn.Domain.Enums;

namespace TraffiLearn.Infrastructure.Helpers
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var roleNames = Enum.GetValues(typeof(Role))
                                .Cast<Role>()
                                .Select(r => r.ToString())
                                .ToArray();

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}
