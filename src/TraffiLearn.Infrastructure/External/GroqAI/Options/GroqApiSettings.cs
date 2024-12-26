using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Infrastructure.External.GroqAI.Options
{
    public sealed class GroqApiSettings
    {
        public const string SectionName = nameof(GroqApiSettings);

        [Required]
        [Url]
        public required string BaseUri { get; init; }

        [Required]
        public required string CreateChatCompletionUri { get; init; }

        [Required]
        public required string Model { get; init; }

        [Required]
        public required string ApiKey { get; init; }
    }
}