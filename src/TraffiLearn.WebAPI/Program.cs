using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Extensions;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Integrations;
using TraffiLearn.Application;
using TraffiLearn.Infrastructure;
using TraffiLearn.Infrastructure.Extensions;
using TraffiLearn.WebAPI.Extensions;
using TraffiLearn.WebAPI.Extensions.DI;
using TraffiLearn.WebAPI.Middleware;

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

            builder.Services.ConfigureSwaggerGen();

            builder.Services.AddApplication(builder.Configuration);
            builder.Services.AddInfrastructure();

            builder.Services.AddPresentationOptions();

            builder.Services.ConfigureAuthentication();

            builder.Services.ConfigureAuthorization();

            var app = builder.Build();

            app.UseExceptionHandlingMiddleware();

            if (!app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            app.UseSwagger();
            app.UseSwaggerUI();
            app.ApplyMigration();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.SeedRoles().Wait();
            app.SeedSuperUserIfNotSeeded().Wait();

            app.Run();
        }
    }
}
