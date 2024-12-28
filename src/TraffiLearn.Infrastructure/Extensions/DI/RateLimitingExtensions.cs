using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using System.Threading.RateLimiting;
using TraffiLearn.Domain.Users.Roles;
using TraffiLearn.Infrastructure.RateLimiting.Options;

namespace TraffiLearn.Infrastructure.Extensions.DI
{
    public static class RateLimitingExtensions
    {
        public const string DefaultPolicyName = "fixed-default";

        public static IServiceCollection AddRateLimiting(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var rateLimitingSettings = GetRateLimitingSettings(configuration);
            var defaultPolicySettings = GetDefaultLimitingPolicySettings(configuration);

            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = rateLimitingSettings?.RejectionStatusCode
                    ?? StatusCodes.Status429TooManyRequests;

                AddDefaultPolicy(options, defaultPolicySettings);
            });

            return services;
        }

        private static RateLimitingSettings? GetRateLimitingSettings(
            IConfiguration configuration)
        {
            return configuration.GetRequiredSection(
                RateLimitingSettings.SectionName)
                    .Get<RateLimitingSettings>();
        }

        private static DefaultRateLimitingPolicySettings? GetDefaultLimitingPolicySettings(
            IConfiguration configuration)
        {
            return configuration.GetRequiredSection(
                DefaultRateLimitingPolicySettings.SectionName)
                    .Get<DefaultRateLimitingPolicySettings>();
        }

        private static void AddDefaultPolicy(
            RateLimiterOptions options,
            DefaultRateLimitingPolicySettings? settings)
        {
            options.AddPolicy(DefaultPolicyName, context =>
            {
                var user = context.User;

                if (IsAdminOrOwner(user))
                {
                    return RateLimitPartition.GetNoLimiter(
                        partitionKey: "no-limit");
                }

                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = settings?.PermitLimit ?? 100,
                        Window = TimeSpan.FromSeconds(settings?.WindowInSeconds ?? 30),
                    });
            });
        }

        private static bool IsAdminOrOwner(ClaimsPrincipal user)
        {
            return user.Identity is not null && user.Identity.IsAuthenticated &&
                (user.IsInRole(Role.Owner.ToString()) || user.IsInRole(Role.Admin.ToString()));
        }
    }
}
