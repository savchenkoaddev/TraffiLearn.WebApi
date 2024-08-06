using System.Security.Claims;
using TraffiLearn.Application.Identity;
using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Application.Abstractions.Auth
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);

        string GenerateRefreshToken();

        ClaimsPrincipal ValidateToken(string token);
    }
}
