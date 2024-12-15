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
    }
}
