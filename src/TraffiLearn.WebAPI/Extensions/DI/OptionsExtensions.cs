using TraffiLearn.Infrastructure.Extensions.DI.Shared;
using TraffiLearn.WebAPI.Options;

namespace TraffiLearn.WebAPI.Extensions.DI
{
    internal static class OptionsExtensions
    {
        public static IServiceCollection AddPresentationOptions(this IServiceCollection services)
        {
            services.ConfigureValidatableOnStartOptions<SuperUserSettings>(
                SuperUserSettings.SectionName);

            return services;
        }
    }
}
