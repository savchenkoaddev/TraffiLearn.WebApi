using TraffiLearn.Application.Abstractions.Identity;

namespace TraffiLearn.Infrastructure.Services.GoogleAuth
{
    internal sealed class GoogleAuthService : IGoogleAuthService
    {
        public Task<bool> IsValidIdToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}
