using Microsoft.Extensions.DependencyInjection;
using TraffiLearn.Application.Questions.Options;
using TraffiLearn.Infrastructure.Authentication.Options;
using TraffiLearn.Infrastructure.Extensions.DI.Shared;
using TraffiLearn.Infrastructure.External.Blobs.Options;
using TraffiLearn.Infrastructure.External.GroqAI.Options;
using TraffiLearn.Infrastructure.Persistence.Options;

namespace TraffiLearn.Infrastructure.Extensions.DI
{
    internal static class AppOptionsExtensions
    {
        public static IServiceCollection AddAppOptions(
            this IServiceCollection services)
        {
            services.ConfigureValidatableOnStartOptions<DbSettings>(DbSettings.SectionName);
            services.ConfigureValidatableOnStartOptions<AzureBlobStorageSettings>(AzureBlobStorageSettings.SectionName);
            services.ConfigureValidatableOnStartOptions<JwtSettings>(JwtSettings.SectionName);
            services.ConfigureValidatableOnStartOptions<QuestionsSettings>(QuestionsSettings.SectionName);
            services.ConfigureValidatableOnStartOptions<GroqApiSettings>(GroqApiSettings.SectionName);

            return services;
        }
    }
}
