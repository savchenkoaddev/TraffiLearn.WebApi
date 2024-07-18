using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Infrastructure.Database;
using TraffiLearn.Infrastructure.External;
using TraffiLearn.Infrastructure.Options;
using TraffiLearn.Infrastructure.Repositories;

namespace TraffiLearn.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var sqlServerSettings = configuration.GetRequiredSection(SqlServerSettings.CONFIG_KEY).Get<SqlServerSettings>();
            var blobStorageSettings = configuration.GetRequiredSection(AzureBlobStorageSettings.CONFIG_KEY).Get<AzureBlobStorageSettings>();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(sqlServerSettings.ConnectionString);
            });

            services.Configure<AzureBlobStorageSettings>(
                configuration.GetRequiredSection(AzureBlobStorageSettings.CONFIG_KEY));

            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

            services.AddSingleton<IBlobService, AzureBlobService>();
            services.AddSingleton(_ => new BlobServiceClient(blobStorageSettings.ConnectionString));

            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();

            return services;
        }
    }
}
