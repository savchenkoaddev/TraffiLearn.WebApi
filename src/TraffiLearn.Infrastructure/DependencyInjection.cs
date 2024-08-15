using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Application.Identity;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Infrastructure.Database;
using TraffiLearn.Infrastructure.External;
using TraffiLearn.Infrastructure.Helpers;
using TraffiLearn.Infrastructure.Options;
using TraffiLearn.Infrastructure.Repositories;
using TraffiLearn.Infrastructure.Services;

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

            services.AddInfrastructureServices();

            services.AddPersistence();
            SeedRoles(services).Wait();
            services.AddRepositories();

            return services;
        }

        private static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IUserContextService<Guid>, UserContextService>();
            services.AddScoped<IRoleService<IdentityRole>, RoleService<IdentityRole>>();
            services.AddScoped<IIdentityService<ApplicationUser>, IdentityService<ApplicationUser>>();
            services.AddScoped<ITokenService, JwtTokenService>();

            return services;
        }

        private async static Task SeedRoles(IServiceCollection services)
        {
            using (var sp = services.BuildServiceProvider())
            {
                var roleService = sp.GetRequiredService<IRoleService<IdentityRole>>();
                await RoleSeeder.SeedRolesAsync(roleService);
            }
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>();
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddRoleManager<RoleManager<IdentityRole>>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();

            return services;
        }

        private static IServiceCollection AddOptions(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<DbSettings>(configuration.GetRequiredSection(DbSettings.SectionName));
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
