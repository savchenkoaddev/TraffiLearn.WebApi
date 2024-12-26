using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Infrastructure.External.GoogleAuth.Options
{
    public sealed class GoogleAuthSettings
    {
        public const string SectionName = nameof(GoogleAuthSettings);

        [Required]
        public required string ClientId { get; init; }
    }
}
