using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Infrastructure.RateLimiting.Options
{
    internal sealed class RateLimitingSettings
    {
        public const string SectionName = nameof(RateLimitingSettings);

        [Required]
        [Range(StatusCodes.Status400BadRequest, StatusCodes.Status511NetworkAuthenticationRequired)]
        public int RejectionStatusCode { get; init; }
    }
}
