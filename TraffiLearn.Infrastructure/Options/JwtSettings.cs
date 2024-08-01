using Microsoft.IdentityModel.Tokens;

namespace TraffiLearn.Infrastructure.Options
{
    public sealed class JwtSettings
    {
        public const string SectionName = nameof(JwtSettings);

        public string? Issuer { get; set; }

        public string? Audience { get; set; }

        public string? SecretKey { get; set; }

        public int ExpirationTimeInHours { get; set; } = 1;

        public string SecurityAlgorithm = SecurityAlgorithms.HmacSha256;
    }
}
