using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TraffiLearn.Application.Extensions.DI;

namespace TraffiLearn.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMappers();

            services.AddMediatR();

            services.AddApplicationServices();

            services.AddPipelineBehaviors();
            services.AddValidators();

            return services;
        }
    }
}
