using Microsoft.Extensions.DependencyInjection;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Services;

namespace TraffiLearn.Application.Extensions.DI
{
    internal static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();

            return services;
        }
    }
}
