using Microsoft.Extensions.DependencyInjection;

namespace TraffiLearn.Infrastructure.Extensions.DI.Shared
{
    public static class SharedExtensions
    {
        public static IServiceCollection ConfigureValidatableOnStartOptions<TOptions>(
           this IServiceCollection services,
           string configSectionPath)
           where TOptions : class
        {
            services.AddOptions<TOptions>()
                .BindConfiguration(configSectionPath)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            return services;
        }
    }
}
