using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Infrastructure.Options
{
    public sealed class JwtSettings
    {
        public const string SectionName = nameof(JwtSettings);

        [Required]
        [StringLength(200)]
        public string? Issuer { get; set; }

        [Required]
        [StringLength(200)]
        public string? Audience { get; set; }

        [Required]
        public string? SecretKey { get; set; }

        [Range(1, 1000)]
        public int ExpirationTimeInMinutes { get; set; } = 20;

        public string SecurityAlgorithm = SecurityAlgorithms.HmacSha256;
    }
}
