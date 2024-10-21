using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Infrastructure.Services.GoogleAuth.Options
{
    public sealed class GoogleAuthSettings
    {
        public const string SectionName = nameof(GoogleAuthSettings);

        [Required]
        public string? ClientId { get; init; }
    }
}
