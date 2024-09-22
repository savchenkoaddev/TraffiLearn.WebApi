using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TraffiLearn.Infrastructure.Authentication.Options;

namespace TraffiLearn.WebAPI.Extensions.DI
{
    internal static class AuthenticationExtensions
    {
        public static IServiceCollection ConfigureAuthentication(
            this IServiceCollection services,
            ConfigurationManager configuration)
        {
            var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>();
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));

            services.AddAuthenticationWithJwtBearer(jwtSettings, signingKey);

            return services;
        }

        private static IServiceCollection AddAuthenticationWithJwtBearer(
            this IServiceCollection services,
            JwtSettings jwtSettings,
            SymmetricSecurityKey signingKey)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = signingKey
                };
            });

            return services;
        }
    }
}
