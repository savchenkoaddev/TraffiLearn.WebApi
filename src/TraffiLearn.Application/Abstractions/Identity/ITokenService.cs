using System.Security.Claims;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;

namespace TraffiLearn.Application.Abstractions.Identity
{
    public interface ITokenService
    {
        string GenerateAccessToken(UserId user);

        string GenerateRefreshToken();

        ClaimsPrincipal ValidateToken(string token);
    }
}
