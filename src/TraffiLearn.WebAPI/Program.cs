using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Extensions;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Integrations;
using System.Text.Json.Serialization;
using TraffiLearn.Application;
using TraffiLearn.Infrastructure;
using TraffiLearn.Infrastructure.Extensions;
using TraffiLearn.WebAPI.Extensions;
using TraffiLearn.WebAPI.Extensions.DI;
using TraffiLearn.WebAPI.Factories;
using TraffiLearn.WebAPI.Middleware;

namespace TraffiLearn.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            builder.Services.AddControllers().AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            builder.Services.AddJsonMultipartFormDataSupport(JsonSerializerChoice.Newtonsoft);

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.ConfigureSwaggerGen();

            builder.Services.AddApplication(config);
            builder.Services.AddInfrastructure(config);

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddSingleton<ProblemDetailsFactory>();

            builder.Services.AddPresentationOptions();

            builder.Services.ConfigureAuthentication();

            builder.Services.ConfigureAuthorization();

            builder.Services.AddCorsPolicies();

            var app = builder.Build();

            app.UseExceptionHandlingMiddleware();

            if (app.Environment.IsDevelopment())
            {
                app.UseCors(CorsExtensions.DevelopmentPolicyName);
            }
            else
            {
                app.UseHttpsRedirection();
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI();
            app.ApplyMigrations();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRateLimiter();

            app.MapControllers();

            app.MapHealthChecks("health", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.SeedRoles().Wait();
            app.SeedSuperUserIfNotSeeded().Wait();

            app.Run();
        }
    }
}
