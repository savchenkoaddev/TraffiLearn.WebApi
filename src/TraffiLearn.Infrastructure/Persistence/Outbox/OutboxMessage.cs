namespace TraffiLearn.Infrastructure.Persistence.Outbox
{
    internal sealed class OutboxMessage
    {
        public Guid Id { get; init; }

        public string Type { get; init; } = string.Empty;

        public string Content { get; init; } = string.Empty;

        public DateTime OccurredOnUtc { get; init; }

        public DateTime? ProcessedOnUtc { get; init; }

        public string? Error { get; init; }
    }
}
