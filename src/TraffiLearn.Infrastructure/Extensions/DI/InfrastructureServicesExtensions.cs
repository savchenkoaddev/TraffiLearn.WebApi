using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Mail;
using TraffiLearn.Application.Abstractions.Emails;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Abstractions.Security;
using TraffiLearn.Application.Users.Identity;
using TraffiLearn.Infrastructure.Extensions.DI.Shared;
using TraffiLearn.Infrastructure.Services;
using TraffiLearn.Infrastructure.Services.Emails;
using TraffiLearn.Infrastructure.Services.Emails.Options;

namespace TraffiLearn.Infrastructure.Extensions.DI
{
    internal static class InfrastructureServicesExtensions
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services)
        {
            services.AddScoped<IUserContextService<Guid>, UserContextService>();
            services.AddScoped<IRoleService<IdentityRole>, RoleService<IdentityRole>>();
            services.AddScoped<ITokenService, JwtTokenService>();
            services.AddScoped<IIdentityService<ApplicationUser>, IdentityService>();
            services.AddScoped<IHasher, Sha256Hasher>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IConfirmationTokenGenerator, ConfirmationTokenGenerator>();
            services.AddScoped<IEmailConfirmationLinkGenerator, EmailConfirmationLinkGenerator>();

            services.AddFluentEmailSender();

            return services;
        }

        private static IServiceCollection AddFluentEmailSender(
           this IServiceCollection services)
        {
            var smtpClientSettings = services.BuildServiceProvider()
                .GetOptions<SmtpClientSettings>();

            SmtpClient smtpClient = CreateSmtpClient(smtpClientSettings);

            services
                .AddFluentEmail(defaultFromEmail: smtpClientSettings.Username)
                .AddSmtpSender(smtpClient);

            return services;
        }

        private static SmtpClient CreateSmtpClient(SmtpClientSettings smtpClientSettings)
        {
            return new SmtpClient(smtpClientSettings.Host)
            {
                Port = smtpClientSettings.Port,
                EnableSsl = smtpClientSettings.EnableSsl,
                Credentials = CreateNetworkCredentials(smtpClientSettings),
            };
        }

        private static NetworkCredential CreateNetworkCredentials(
            SmtpClientSettings smtpClientSettings)
        {
            return new NetworkCredential(
                userName: smtpClientSettings.Username,
                password: smtpClientSettings.Password);
        }
    }
}
