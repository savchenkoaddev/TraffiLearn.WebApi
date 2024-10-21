using Microsoft.Extensions.Options;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Infrastructure.Services.GoogleAuth.Options;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace TraffiLearn.Infrastructure.Services.GoogleAuth
{
    internal sealed class GoogleAuthService : IGoogleAuthService
    {
        private readonly GoogleAuthSettings _settings;

        public GoogleAuthService(IOptions<GoogleAuthSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<string> ValidateIdTokenAndGetEmailAsync(string token)
        {
            var validationSettings = GetValidationSettings();

            var payload = await ValidateAsync(
                jwt: token,
                validationSettings);

            return payload.Email;
        }

        private ValidationSettings GetValidationSettings()
        {
            return new ValidationSettings()
            {
                Audience = [_settings.ClientId]
            };
        }
    }
}
