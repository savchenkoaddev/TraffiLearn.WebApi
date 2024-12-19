using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Infrastructure.Helpers;
using TraffiLearn.Infrastructure.Persistence;

namespace TraffiLearn.Infrastructure.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            dbContext.Database.Migrate();
        }

        public async static Task SeedRoles(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var roleService = scope.ServiceProvider.GetRequiredService<IRoleService<IdentityRole>>();

            await RoleSeeder.SeedRolesAsync(roleService);
        }
    }
}
