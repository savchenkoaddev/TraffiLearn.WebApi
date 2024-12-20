using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Net.Mime;
using TraffiLearn.Application.Abstractions.AI;
using TraffiLearn.Infrastructure.Extensions.DI.Shared;
using TraffiLearn.Infrastructure.External.GroqAI;
using TraffiLearn.Infrastructure.External.GroqAI.Options;

namespace TraffiLearn.Infrastructure.Extensions.DI
{
    internal static class HttpClientsExtensions
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient<IAIService, GroqApiService>(
                ConfigureClientWithOptions);

            return services;
        }

        private static void ConfigureClientWithOptions(
            IServiceProvider sp, HttpClient client)
        {
            var groqApiSettings = sp.GetOptions<GroqApiSettings>();

            client.DefaultRequestHeaders.Add(
                HeaderNames.Accept,
                MediaTypeNames.Application.Json);

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                JwtBearerDefaults.AuthenticationScheme,
                groqApiSettings.ApiKey);

            client.BaseAddress = new Uri(groqApiSettings.BaseUri);
        }
    }
}
