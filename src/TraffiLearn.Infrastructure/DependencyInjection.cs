using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Net.Mime;
using TraffiLearn.Application.Abstractions.AI;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Application.Questions.Options;
using TraffiLearn.Application.Users.Identity;
using TraffiLearn.Domain.Aggregates.Comments;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Tickets;
using TraffiLearn.Domain.Aggregates.Topics;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Infrastructure.Authentication.Options;
using TraffiLearn.Infrastructure.External.Blobs;
using TraffiLearn.Infrastructure.External.Blobs.Options;
using TraffiLearn.Infrastructure.External.GroqAI;
using TraffiLearn.Infrastructure.External.GroqAI.Options;
using TraffiLearn.Infrastructure.Persistence;
using TraffiLearn.Infrastructure.Persistence.Options;
using TraffiLearn.Infrastructure.Persistence.Repositories;
using TraffiLearn.Infrastructure.Services;

namespace TraffiLearn.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services)
        {
            services.AddOptions();

            services.AddExternalServices();

            services.AddInfrastructureServices();

            services.AddPersistence();
            services.AddRepositories();

            services.AddHttpClients();

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

        private static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                (serviceProvider, options) =>
            {
                var dbSettings = serviceProvider.GetRequiredService
                    <IOptions<DbSettings>>().Value;

                options.UseSqlServer(dbSettings.ConnectionString);
            });

            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddRoleManager<RoleManager<IdentityRole>>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();

            return services;
        }

        private static IServiceCollection AddOptions(
            this IServiceCollection services)
        {
            services.ConfigureValidatableOnStartOptions<DbSettings>(DbSettings.SectionName);
            services.ConfigureValidatableOnStartOptions<AzureBlobStorageSettings>(AzureBlobStorageSettings.SectionName);
            services.ConfigureValidatableOnStartOptions<JwtSettings>(JwtSettings.SectionName);
            services.ConfigureValidatableOnStartOptions<QuestionsSettings>(QuestionsSettings.SectionName);
            services.ConfigureValidatableOnStartOptions<GroqApiSettings>(GroqApiSettings.SectionName);

            return services;
        }

        public static IServiceCollection ConfigureValidatableOnStartOptions<TOptions>(
            this IServiceCollection services,
            string configSectionPath)
            where TOptions : class
        {
            services.AddOptions<TOptions>()
                .BindConfiguration(configSectionPath)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            return services;
        }

        private static IServiceCollection AddExternalServices(this IServiceCollection services)
        {
            services.AddSingleton((serviceProvider) =>
            {
                var blobStorageSettings = serviceProvider.GetRequiredService<IOptions<AzureBlobStorageSettings>>().Value;

                var blobServiceClient = new BlobServiceClient(blobStorageSettings.ConnectionString);

                var containerClient = blobServiceClient.GetBlobContainerClient(blobStorageSettings.ContainerName);

                containerClient.CreateIfNotExists();

                var properties = containerClient.GetProperties();

                if (properties.Value.PublicAccess != PublicAccessType.Blob)
                {
                    containerClient.SetAccessPolicy(PublicAccessType.Blob);
                }

                return blobServiceClient;
            });

            services.AddSingleton<IBlobService, AzureBlobService>();

            services.AddScoped<IAIService, GroqApiService>();

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

        private static IServiceCollection AddHttpClients(this IServiceCollection services)
        {
            var groqApiSettings = services.BuildServiceProvider().GetRequiredService<IOptions<GroqApiSettings>>().Value;

            services.AddHttpClient<IAIService, GroqApiService>(ConfigureClientWithOptions(groqApiSettings));

            return services;
        }

        private static Action<HttpClient> ConfigureClientWithOptions(GroqApiSettings groqAISettings)
        {
            return options =>
            {
                options.DefaultRequestHeaders.Add(
                    HeaderNames.Accept,
                    MediaTypeNames.Application.Json);

                options.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    JwtBearerDefaults.AuthenticationScheme,
                    groqAISettings.ApiKey);

                options.BaseAddress = new Uri(groqAISettings.BaseUri);
            };
        }
    }
}
