using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TraffiLearn.Application;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.Infrastructure;
using TraffiLearn.Infrastructure.Authentication;
using TraffiLearn.Infrastructure.Authentication.Options;
using TraffiLearn.Infrastructure.Extensions;
using TraffiLearn.WebAPI.Middleware;

namespace TraffiLearn.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddApplication(builder.Configuration);
            builder.Services.AddInfrastructure();

            ConfigureAuthentication(
                builder.Services,
                builder.Configuration);

            ConfigureAuthorization(builder.Services);

            var app = builder.Build();

            app.UseExceptionHandlingMiddleware();

            app.UseHttpsRedirection();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.ApplyMigration();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.SeedRoles().Wait();

            app.Run();
        }

        private static void ConfigureAuthorization(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    Permission.AccessSpecificUserData.ToString(),
                    policy =>
                    {
                        policy.RequireRole(Role.Admin.ToString());
                    });

                options.AddPolicy(
                    Permission.AccessData.ToString(),
                    policy =>
                    {
                        policy.RequireAuthenticatedUser();
                    });

                options.AddPolicy(
                    Permission.DowngradeAccounts.ToString(),
                    policy =>
                    {
                        policy.RequireRole(Role.Owner.ToString());
                    });

                options.AddPolicy(
                    Permission.RegisterAdmins.ToString(),
                    policy =>
                    {
                        policy.RequireRole(Role.Owner.ToString());
                    });

                options.AddPolicy(
                    Permission.RemoveAdmins.ToString(),
                    policy =>
                    {
                        policy.RequireRole(Role.Owner.ToString());
                    });

                options.AddPolicy(
                    Permission.ModifyData.ToString(),
                    policy =>
                    {
                        policy.RequireRole(
                            roles: [
                                Role.Admin.ToString(),
                                Role.Owner.ToString()
                            ]);
                    });
            });
        }

        private static void ConfigureAuthentication(
            IServiceCollection services,
            ConfigurationManager configuration)
        {
            var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>();
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));

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
        }
    }
}
