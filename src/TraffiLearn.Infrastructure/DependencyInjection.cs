using Microsoft.Extensions.DependencyInjection;
using TraffiLearn.Infrastructure.Extensions.DI;

namespace TraffiLearn.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services)
        {
            services.AddAppOptions();

            services.AddExternalServices();

            services.AddInfrastructureServices();

            services.AddPersistence();

            services.AddRepositories();

            services.AddHttpClients();

            return services;
        }
    }
}
