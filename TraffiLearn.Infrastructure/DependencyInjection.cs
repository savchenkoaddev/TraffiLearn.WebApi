using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
            services.AddOptions(configuration);
            
            services.AddExternalServices();

            services.AddPersistance();
            services.AddRepositories();
            
            return services;
        }

        private static IServiceCollection AddPersistance(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>();
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

            return services;
        }

        private static IServiceCollection AddOptions(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<SqlServerSettings>(configuration.GetRequiredSection(SqlServerSettings.SectionName));
            services.Configure<AzureBlobStorageSettings>(configuration.GetRequiredSection(AzureBlobStorageSettings.SectionName));

            return services;
        }

        private static IServiceCollection AddExternalServices(this IServiceCollection services)
        {
            services.AddSingleton((serviceProvider) =>
            {
                var blobStorageSettings = serviceProvider.GetRequiredService<IOptions<AzureBlobStorageSettings>>().Value;

                return new BlobServiceClient(blobStorageSettings.ConnectionString);
            });

            services.AddSingleton<IBlobService, AzureBlobService>();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ITopicRepository, TopicRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
