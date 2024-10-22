using Google.Apis.Auth;
using Microsoft.Extensions.Options;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.Aggregates.Users.Errors;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Infrastructure.External.GoogleAuth.Options;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace TraffiLearn.Infrastructure.External.GoogleAuth
{
    internal sealed class GoogleAuthService : IGoogleAuthService
    {
        private readonly GoogleAuthSettings _settings;

        public GoogleAuthService(IOptions<GoogleAuthSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<Result<Email>> ValidateIdTokenAsync(string token)
        {
            var validationSettings = GetValidationSettings();

            try
            {
                var payload = await ValidateAsync(
                    jwt: token,
                    validationSettings);

                var emailResult = Email.Create(payload.Email);

                if (emailResult.IsFailure)
                {
                    throw new InvalidOperationException("Google API responded with an invalid email.");
                }

                return emailResult.Value;
            }
            catch (InvalidJwtException)
            {
                return Result.Failure<Email>(UserErrors.InvalidGoogleIdToken);
            }
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
