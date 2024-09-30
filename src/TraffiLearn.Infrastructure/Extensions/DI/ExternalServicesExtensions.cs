using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Mail;
using TraffiLearn.Application.Abstractions.AI;
using TraffiLearn.Application.Abstractions.Emails;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Infrastructure.Extensions.DI.Shared;
using TraffiLearn.Infrastructure.External.Blobs;
using TraffiLearn.Infrastructure.External.Blobs.Options;
using TraffiLearn.Infrastructure.External.Emails;
using TraffiLearn.Infrastructure.External.Emails.Options;
using TraffiLearn.Infrastructure.External.GroqAI;

namespace TraffiLearn.Infrastructure.Extensions.DI
{
    internal static class ExternalServicesExtensions
    {
        public static IServiceCollection AddExternalServices(
            this IServiceCollection services)
        {
            services.AddBlobServiceClient();

            services.AddFluentEmailSender();

            services.AddScoped<IEmailSender, EmailSender>();

            services.AddSingleton<IBlobService, AzureBlobService>();

            services.AddScoped<IAIService, GroqApiService>();

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

        private static bool ContainerHasPublicAccess(
            BlobContainerProperties properties)
        {
            return properties.PublicAccess != PublicAccessType.Blob;
        }

        private static IServiceCollection AddFluentEmailSender(
            this IServiceCollection services)
        {
            var smtpClientSettings = services.BuildServiceProvider()
                .GetOptions<SmtpClientSettings>();

            SmtpClient smtpClient = CreateSmtpClient(smtpClientSettings);

            services
                .AddFluentEmail(defaultFromEmail: smtpClientSettings.Username)
                .AddSmtpSender(smtpClient);

            return services;
        }

        private static SmtpClient CreateSmtpClient(SmtpClientSettings smtpClientSettings)
        {
            return new SmtpClient(smtpClientSettings.Host)
            {
                Port = smtpClientSettings.Port,
                EnableSsl = true,
                Credentials = CreateNetworkCredentials(smtpClientSettings),
            };
        }

        private static NetworkCredential CreateNetworkCredentials(
            SmtpClientSettings smtpClientSettings)
        {
            return new NetworkCredential(
                userName: smtpClientSettings.Username,
                password: smtpClientSettings.Password);
        }
    }
}
