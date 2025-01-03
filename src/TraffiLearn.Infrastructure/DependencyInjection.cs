using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TraffiLearn.Infrastructure.Extensions.DI;

namespace TraffiLearn.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAppOptions();

            services.AddMessageBroker();

            services.AddExternalServices();

            services.AddInfrastructureServices();

            services.AddBackgroundJobs();

            services.AddPersistence();

            services.AddRepositories();

            services.AddHttpClients();

            services.AddAppHealthChecks();

            services.AddRateLimiting(configuration);

            return services;
        }
    }
}
