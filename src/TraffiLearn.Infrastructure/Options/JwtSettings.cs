using Microsoft.IdentityModel.Tokens;

namespace TraffiLearn.Infrastructure.Options
{
    public sealed class JwtSettings
    {
        public const string SectionName = nameof(JwtSettings);

        public string? Issuer { get; set; }

        public string? Audience { get; set; }

        public string? SecretKey { get; set; }

        public int ExpirationTimeInMinutes { get; set; } = 20;

        public string SecurityAlgorithm = SecurityAlgorithms.HmacSha256;
    }
}
