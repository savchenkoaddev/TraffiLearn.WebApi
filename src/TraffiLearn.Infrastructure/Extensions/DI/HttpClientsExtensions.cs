using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Net.Mime;
using TraffiLearn.Application.Abstractions.AI;
using TraffiLearn.Infrastructure.External.GroqAI.Options;
using TraffiLearn.Infrastructure.External.GroqAI;

namespace TraffiLearn.Infrastructure.Extensions.DI
{
    internal static class HttpClientsExtensions
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient<IAIService, GroqApiService>(
                ConfigureClientWithOptions(services));

            return services;
        }

        private static Action<HttpClient> ConfigureClientWithOptions(IServiceCollection services)
        {
            var groqApiSettings = services.BuildServiceProvider().GetOptions<GroqApiSettings>();

            return options =>
            {
                options.DefaultRequestHeaders.Add(
                    HeaderNames.Accept,
                    MediaTypeNames.Application.Json);

                options.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    JwtBearerDefaults.AuthenticationScheme,
                    groqApiSettings.ApiKey);

                options.BaseAddress = new Uri(groqApiSettings.BaseUri);
            };
        }
    }
}
