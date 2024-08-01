using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TraffiLearn.Application;
using TraffiLearn.Infrastructure;
using TraffiLearn.Infrastructure.Options;
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
            builder.Services.AddInfrastructure(builder.Configuration);

            builder.Services.AddAuthorizationBuilder();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>();

                    var signingKey = new SymmetricSecurityKey(
               Encoding.UTF8.GetBytes(jwtSettings.SecretKey));

                    options.TokenValidationParameters = new()
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

            var app = builder.Build();

            app.UseExceptionHandlingMiddleware();

            app.UseHttpsRedirection();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
