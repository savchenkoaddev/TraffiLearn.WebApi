using Microsoft.Extensions.Options;
using TraffiLearn.Application.Abstractions.Emails;
using TraffiLearn.Infrastructure.Services.Emails.Options;

namespace TraffiLearn.Infrastructure.Services.Emails
{
    internal sealed class EmailConfirmationLinkGenerator : IEmailConfirmationLinkGenerator
    {
        private readonly EmailConfirmationLinkGeneratorSettings _settings;

        public EmailConfirmationLinkGenerator(IOptions<EmailConfirmationLinkGeneratorSettings> settings)
        {
            _settings = settings.Value;
        }

        public string Generate(string userId, string token)
        {
            return $"{_settings.BaseConfirmationEndpointUri}?" +
                $"{_settings.UserIdParameterName}={userId}&" +
                $"{_settings.TokenParameterName}={token}";
        }
    }
}
