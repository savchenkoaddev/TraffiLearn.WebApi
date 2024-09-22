using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Users.Identity;
using TraffiLearn.Infrastructure.Services;

namespace TraffiLearn.Infrastructure.Extensions.DI
{
    internal static class InfrastructureServicesExtensions
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services)
        {
            services.AddScoped<IUserContextService<Guid>, UserContextService>();
            services.AddScoped<IRoleService<IdentityRole>, RoleService<IdentityRole>>();
            services.AddScoped<IIdentityService<ApplicationUser>, IdentityService<ApplicationUser>>();
            services.AddScoped<ITokenService, JwtTokenService>();

            return services;
        }
    }
}
