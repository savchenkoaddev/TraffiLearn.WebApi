using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Abstractions.Identity
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);

        string GenerateRefreshToken();

        Task<Result> ValidateAccessTokenAsync(
            string token, bool validateLifetime = true);
    }
}
