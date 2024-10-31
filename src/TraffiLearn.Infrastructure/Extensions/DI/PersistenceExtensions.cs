using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Users.Identity;
using TraffiLearn.Infrastructure.Extensions.DI.Shared;
using TraffiLearn.Infrastructure.Persistence;
using TraffiLearn.Infrastructure.Persistence.Interceptors;
using TraffiLearn.Infrastructure.Persistence.Options;

namespace TraffiLearn.Infrastructure.Extensions.DI
{
    internal static class PersistenceExtensions
    {
        public static IServiceCollection AddPersistence(
            this IServiceCollection services)
        {
            services.RegisterInterceptors();

            services.AddAppDbContext();

            services.AddUnitOfWork();

            services.AddAppIdentity();

            return services;
        }

        private static IServiceCollection RegisterInterceptors(
            this IServiceCollection services)
        {
            return services.RegisterInterceptor
                <ConvertDomainEventsToOutboxMessagesInterceptor>();
        }
         
        private static IServiceCollection RegisterInterceptor<TInterceptor>(
            this IServiceCollection services)
            where TInterceptor : class, IInterceptor
        {
            return services.AddSingleton<TInterceptor>();
        }

        private static void AddAppDbContext(
            this IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                (serviceProvider, options) =>
                {
                    options
                        .UseNpgsqlWithDbSettings(serviceProvider)
                        .AddAppInterceptors(serviceProvider);
                });
        }

        private static DbContextOptionsBuilder UseNpgsqlWithDbSettings(
            this DbContextOptionsBuilder options,
            IServiceProvider serviceProvider)
        {
            var dbSettings = serviceProvider.GetOptions<DbSettings>();

            return options.UseNpgsql(dbSettings.ConnectionString);
        }

        private static DbContextOptionsBuilder AddAppInterceptors(
            this DbContextOptionsBuilder options,
            IServiceProvider serviceProvider)
        {
            var publishDomainEventsInterceptor = serviceProvider
                .GetRequiredService<ConvertDomainEventsToOutboxMessagesInterceptor>();

            return options.AddInterceptors(publishDomainEventsInterceptor);
        }

        private static IServiceCollection AddUnitOfWork(
            this IServiceCollection services)
        {
            return services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        private static IdentityBuilder AddAppIdentity(
            this IServiceCollection services)
        {
            return services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Tokens.AuthenticatorTokenProvider = "Default";
            })
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        }
    }
}
