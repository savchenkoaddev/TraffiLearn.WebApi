using System.Security.Claims;
using TraffiLearn.Application.Identity;

namespace TraffiLearn.Application.Abstractions.Auth
{
    public interface ITokenService
    {
        string GenerateAccessToken(ApplicationUser identityUser);

        string GenerateRefreshToken();

        ClaimsPrincipal ValidateToken(string token);
    }
}
