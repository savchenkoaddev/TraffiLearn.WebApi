using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Infrastructure.BackgroundJobs.Options
{
    public sealed class PlanExpiryNotificationSettings
    {
        public const string SectionName = nameof(PlanExpiryNotificationSettings);

        [Required]
        [Range(0, 23)]
        public int Hours { get; init; }

        [Required]
        [Range(0, 59)]
        public int Minutes { get; init; }
    }
}
