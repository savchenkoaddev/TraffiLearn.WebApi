using Microsoft.Extensions.Options;
using System.Text;
using TraffiLearn.Application.Abstractions.Emails;
using TraffiLearn.Infrastructure.Services.Emails.Options;

namespace TraffiLearn.Infrastructure.Services.Emails
{
    internal sealed class EmailLinkGenerator : IEmailLinkGenerator
    {
        private readonly EmailLinkGeneratorSettings _settings;

        public EmailLinkGenerator(IOptions<EmailLinkGeneratorSettings> settings)
        {
            _settings = settings.Value;
        }

        public string GenerateConfirmChangeEmailLink(
            string userId, 
            string newEmail, 
            string token)
        {
            StringBuilder builder = new();

            builder.Append($"{_settings.BaseConfirmChangeEmailEndpointUri}?");
            builder.Append($"{_settings.UserIdParameterName}={userId}&");
            builder.Append($"{_settings.TokenParameterName}={token}&");
            builder.Append($"{_settings.NewEmailParameterName}={newEmail}");

            return builder.ToString();
        }

        public string GenerateConfirmationLink(string userId, string token)
        {
            StringBuilder builder = new();

            builder.Append($"{_settings.BaseConfirmationEndpointUri}?");
            builder.Append($"{_settings.UserIdParameterName}={userId}&");
            builder.Append($"{_settings.TokenParameterName}={token}");

            return builder.ToString();
        }

        public string GenerateRecoverPasswordLink(string userId, string token)
        {
            StringBuilder builder = new();

            builder.Append($"{_settings.BaseResetPasswordEndpointUri}?");
            builder.Append($"{_settings.UserIdParameterName}={userId}&");
            builder.Append($"{_settings.TokenParameterName}={token}");

            return builder.ToString();
        }
    }
}
