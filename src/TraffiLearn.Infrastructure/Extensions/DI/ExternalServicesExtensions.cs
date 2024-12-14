using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Stripe;
using TraffiLearn.Application.Abstractions.AI;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Abstractions.Payments;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Infrastructure.Extensions.DI.Shared;
using TraffiLearn.Infrastructure.External.Blobs;
using TraffiLearn.Infrastructure.External.Blobs.Options;
using TraffiLearn.Infrastructure.External.GoogleAuth;
using TraffiLearn.Infrastructure.External.GroqAI;
using TraffiLearn.Infrastructure.Services.Payments;
using TraffiLearn.Infrastructure.Services.Payments.Options;

namespace TraffiLearn.Infrastructure.Extensions.DI
{
    internal static class ExternalServicesExtensions
    {
        public static IServiceCollection AddExternalServices(
            this IServiceCollection services)
        {
            services.AddBlobServiceClient();
            services.AddStripe();

            services.AddSingleton<IBlobService, AzureBlobService>();
            services.AddScoped<IAIService, GroqApiService>();
            services.AddScoped<IGoogleAuthService, GoogleAuthService>();
            services.AddScoped<IStripeWebhookService, StripeWebhookService>();

            return services;
        }

        private static IServiceCollection AddBlobServiceClient(
            this IServiceCollection services)
        {
            services.AddSingleton((serviceProvider) =>
            {
                var blobStorageSettings = serviceProvider.GetOptions<AzureBlobStorageSettings>();

                var blobServiceClient = new BlobServiceClient(blobStorageSettings.ConnectionString);

                var containerClient = blobServiceClient.GetBlobContainerClient(blobStorageSettings.ContainerName);

                containerClient.CreateIfNotExists();

                var properties = containerClient.GetProperties();

                if (ContainerHasPublicAccess(properties.Value))
                {
                    containerClient.SetAccessPolicy(PublicAccessType.Blob);
                }

                return blobServiceClient;
            });

            return services;
        }

        private static IServiceCollection AddStripe(
            this IServiceCollection services)
        {
            services.PostConfigure<StripeSettings>(stripeSettings =>
            {
                StripeConfiguration.ApiKey = stripeSettings.SecretKey;
            });

            services.AddTransient<IPaymentService, StripePaymentService>();

            return services;
        }

        private static bool ContainerHasPublicAccess(
            BlobContainerProperties properties)
        {
            return properties.PublicAccess != PublicAccessType.Blob;
        }
    }
}
