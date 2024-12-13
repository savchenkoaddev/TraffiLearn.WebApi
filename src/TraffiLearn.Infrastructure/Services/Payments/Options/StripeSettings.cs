using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Infrastructure.Services.Payments.Options
{
    public sealed class StripeSettings
    {
        public const string SectionName = nameof(StripeSettings);

        [Required]
        public required string PublishableKey { get; init; }

        [Required]
        public required string SecretKey { get; init; }

        [Url]
        [Required]
        public required string SuccessUrl { get; init; }

        [Url]
        [Required]
        public required string CancelUrl { get; init; }
    }
}
