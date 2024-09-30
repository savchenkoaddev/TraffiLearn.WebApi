using Microsoft.Extensions.Options;
using TraffiLearn.Application.Abstractions.Emails;
using TraffiLearn.Infrastructure.Options;

namespace TraffiLearn.Infrastructure.Services
{
    internal sealed class EmailConfirmationLinkGenerator : IEmailConfirmationLinkGenerator
    {
        private readonly EmailConfirmationLinkGeneratorSettings _emailConfirmationSettings;

        public EmailConfirmationLinkGenerator(
            IOptions<EmailConfirmationLinkGeneratorSettings> emailConfirmationSettings)
        {
            _emailConfirmationSettings = emailConfirmationSettings.Value;
        }

        public string Generate(
            string userId, 
            string token)
        {
            return $"{_emailConfirmationSettings.BaseConfirmationEndpointUri}" +
                $"?{_emailConfirmationSettings.UserIdParameterName}={userId}&" +
                $"{_emailConfirmationSettings.TokenParameterName}={token}";
        }
    }
}
