using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Infrastructure.RateLimiting.Options
{
    internal sealed class DefaultRateLimitingPolicySettings
    {
        public const string SectionName = nameof(DefaultRateLimitingPolicySettings);

        [Required]
        [Range(1, 10000)]
        public int PermitLimit { get; init; }

        [Required]
        [Range(1, 10000)]
        public int WindowInSeconds { get; init; }
    }
}
