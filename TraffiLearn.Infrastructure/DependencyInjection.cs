using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Infrastructure.Database;
using TraffiLearn.Infrastructure.External;
using TraffiLearn.Infrastructure.Options;
using TraffiLearn.Infrastructure.OptionsSetup;
using TraffiLearn.Infrastructure.Repositories;

namespace TraffiLearn.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.ConfigureOptions<SqlServerSettingsSetup>();
            services.ConfigureOptions<AzureBlobStorageSettingsSetup>();

            services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                var sqlServerSettings = serviceProvider.GetRequiredService<IOptions<AzureBlobStorageSettings>>().Value;

                options.UseSqlServer(sqlServerSettings.ConnectionString);
            });

            services.AddSingleton((serviceProvider) =>
            {
                var blobStorageSettings = serviceProvider.GetRequiredService<IOptions<AzureBlobStorageSettings>>().Value;

                return new BlobServiceClient(blobStorageSettings.ConnectionString);
            });

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

            services.AddSingleton<IBlobService, AzureBlobService>();

            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();

            return services;
        }
    }
}
