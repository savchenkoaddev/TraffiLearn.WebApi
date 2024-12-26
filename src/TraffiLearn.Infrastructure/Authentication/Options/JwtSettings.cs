using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Infrastructure.Authentication.Options
{
    public sealed class JwtSettings
    {
        public const string SectionName = nameof(JwtSettings);

        [Required]
        [StringLength(200)]
        public required string Issuer { get; init; }

        [Required]
        [StringLength(200)]
        public required string Audience { get; init; }

        [Required]
        public required string SecretKey { get; init; }

        [Range(1, 1000)]
        public int ExpirationTimeInMinutes { get; init; } = 20;

        public string SecurityAlgorithm = SecurityAlgorithms.HmacSha256;
    }
}
