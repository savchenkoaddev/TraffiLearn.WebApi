using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.Errors;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Infrastructure.Authentication.Options;

namespace TraffiLearn.Infrastructure.Services
{
    internal sealed class JwtTokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly SymmetricSecurityKey _symmetricSecurityKey;

        public JwtTokenService(
            IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
            _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        }

        public string GenerateAccessToken(User user)
        {
            var claims = GenerateClaims(user);

            var signingCredentials = new SigningCredentials(
                 key: _symmetricSecurityKey,
                 algorithm: _jwtSettings.SecurityAlgorithm);

            var token = GenerateJwt(claims, signingCredentials);

            return GetJwtTokenString(token);
        }

        public async Task<Result> ValidateAccessTokenAsync(string token, bool validateLifetime = true)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = GetTokenValidationParameters(
                validateLifetime);

            var validationResult = await tokenHandler.ValidateTokenAsync(
                token, validationParameters);

            if (!validationResult.IsValid)
            {
                return Result.Failure(UserErrors.InvalidAccessToken);
            }

            return Result.Success();
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        #region Private Methods

        private JwtSecurityToken GenerateJwt(
            Claim[] claims,
            SigningCredentials credentials)
        {
            return new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims,
                null,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationTimeInMinutes),
                signingCredentials: credentials);
        }

        private string GetJwtTokenString(JwtSecurityToken token)
        {
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private Claim[] GenerateClaims(User user)
        {
            return
            [
                new(JwtRegisteredClaimNames.Sub, user.Id.Value.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email.Value.ToString()),
                new("role", user.Role.ToString())
            ];
        }

        private TokenValidationParameters GetTokenValidationParameters(
            bool validateLifeTime = true)
        {
            return new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = validateLifeTime,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = _symmetricSecurityKey
            };
        }

        #endregion
    }
}
