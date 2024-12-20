using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Infrastructure.MessageBroker
{
    public sealed class MessageBrokerSettings
    {
        public const string SectionName = nameof(MessageBrokerSettings);

        [Required]
        public required string Host { get; init; }

        [Required]
        public required string Username { get; init; }

        [Required]
        public required string Password { get; init; }

        [Required]
        public required string ConnectionString { get; init; }

        [Required]
        public required string EmailExchangeName { get; init; }

        [Range(1, 100)]
        [Required]
        public int RetryCount { get; init; }

        [Range(100, 60000)]
        [Required]
        public int RetryIntervalMilliseconds { get; init; }
    }
}
