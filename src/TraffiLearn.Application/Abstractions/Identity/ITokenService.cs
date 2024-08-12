using System.Security.Claims;
using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Application.Abstractions.Identity
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);

        string GenerateRefreshToken();

        ClaimsPrincipal ValidateToken(string token);
    }
}
