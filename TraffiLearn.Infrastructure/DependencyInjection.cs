using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TraffiLearn.Application.Abstractions.Auth;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Application.Identity;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Infrastructure.Authentication;
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

            services.AddAuthenticationServices();
            services.AddExternalServices();

            services.AddPersistence();
            services.AddRepositories();
            
            return services;
        }

        private static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, JwtTokenService>();

            return services;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>();
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();

            return services;
        }

        private static IServiceCollection AddOptions(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<SqlServerSettings>(configuration.GetRequiredSection(SqlServerSettings.SectionName));
            services.Configure<AzureBlobStorageSettings>(configuration.GetRequiredSection(AzureBlobStorageSettings.SectionName));
            services.Configure<JwtSettings>(configuration.GetRequiredSection(JwtSettings.SectionName));

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
            services.AddScoped<ICommentRepository, CommentRepository>();

            return services;
        }
    }
}
