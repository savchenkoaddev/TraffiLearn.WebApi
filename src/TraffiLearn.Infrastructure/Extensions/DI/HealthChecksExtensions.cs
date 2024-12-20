using Azure.Storage.Blobs;
using Microsoft.Extensions.DependencyInjection;
using TraffiLearn.Infrastructure.Extensions.DI.Shared;
using TraffiLearn.Infrastructure.External.Blobs.Options;
using TraffiLearn.Infrastructure.MessageBroker;
using TraffiLearn.Infrastructure.Persistence;
using TraffiLearn.Infrastructure.Persistence.Options;

namespace TraffiLearn.Infrastructure.Extensions.DI
{
    internal static class HealthChecksExtensions
    {
        public static IServiceCollection AddAppHealthChecks(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddNpgSql(sp => sp.GetOptions<DbSettings>().ConnectionString)
                .AddDbContextCheck<ApplicationDbContext>()
                .AddRabbitMQ((sp, options) =>
                {
                    var messageBrokerSettings = sp.GetOptions<MessageBrokerSettings>();

                    options.ConnectionUri = new Uri(messageBrokerSettings.ConnectionString);
                })
                .AddAzureBlobStorage(sp =>
                {
                    var azureBlobStorageSettings = sp.GetOptions<AzureBlobStorageSettings>();

                    return new BlobServiceClient(azureBlobStorageSettings.ConnectionString);
                });

            return services;
        }
    }
}
