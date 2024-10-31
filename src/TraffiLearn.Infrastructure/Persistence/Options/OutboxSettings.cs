using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Infrastructure.Persistence.Options
{
    public sealed class OutboxSettings
    {
        public const string SectionName = nameof(OutboxSettings);

        [Required]
        [Range(1, int.MaxValue)]
        public int BatchSize { get; init; }

        [Required]
        [Range(1, int.MaxValue)]
        public int ProcessIntervalInSeconds { get; init; }
    }
}
