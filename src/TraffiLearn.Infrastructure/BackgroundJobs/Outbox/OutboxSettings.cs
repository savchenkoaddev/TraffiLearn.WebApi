using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Infrastructure.BackgroundJobs.Outbox
{
    public sealed class OutboxSettings
    {
        public const string SectionName = nameof(OutboxSettings);

        [Range(1, 1000)]
        public int ProcessMessagesAtATimeCount { get; set; } = 20;

        [Range(1, 10000)]
        public int ProcessWithIntervalInSeconds { get; set; } = 10;
    }
}
