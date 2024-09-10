using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Extensions;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Integrations;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using TraffiLearn.Application;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.Infrastructure;
using TraffiLearn.Infrastructure.Authentication;
using TraffiLearn.Infrastructure.Authentication.Options;
using TraffiLearn.Infrastructure.Extensions;
using TraffiLearn.WebAPI.Extensions;
using TraffiLearn.WebAPI.Middleware;
using TraffiLearn.WebAPI.Options;

namespace TraffiLearn.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddJsonMultipartFormDataSupport(JsonSerializerChoice.Newtonsoft);

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                ConfigureSwaggerAuthorization(options);
            });

            builder.Services.AddApplication(builder.Configuration);
            builder.Services.AddInfrastructure();

            builder.Services.ConfigureValidatableOnStartOptions<SuperUserSettings>(
                SuperUserSettings.SectionName);

            ConfigureAuthentication(
                builder.Services,
                builder.Configuration);

            ConfigureAuthorization(builder.Services);

            var app = builder.Build();

            app.UseExceptionHandlingMiddleware();

            if (!app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

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
            app.SeedSuperUserIfNotSeeded().Wait();

            app.Run();
        }

        private static void ConfigureSwaggerAuthorization(SwaggerGenOptions options)
        {
            options.AddSecurityDefinition(
                JwtBearerDefaults.AuthenticationScheme,
                new OpenApiSecurityScheme
                {
                    Name = HeaderNames.Authorization,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer { token }\""
                });

            options.AddSecurityRequirement(
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        },
                        new string[] { }
                    }
                });
        }

        private static void ConfigureAuthorization(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    Permission.AccessSpecificUserData.ToString(),
                    policy =>
                    {
                        policy.RequireRole(
                            roles: [
                                Role.Admin.ToString(),
                                Role.Owner.ToString()
                            ]);
                    });

                options.AddPolicy(
                    Permission.AccessData.ToString(),
                    policy =>
                    {
                        policy.RequireRole(roles: [
                            Role.RegularUser.ToString(),
                            Role.Admin.ToString(),
                            Role.Owner.ToString(),
                        ]);
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
