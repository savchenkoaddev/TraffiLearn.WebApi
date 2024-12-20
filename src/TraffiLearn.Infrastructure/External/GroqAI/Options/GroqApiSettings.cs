using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Infrastructure.External.GroqAI.Options
{
    public sealed class GroqApiSettings
    {
        public const string SectionName = nameof(GroqApiSettings);

        [Required]
        [Url]
        public required string BaseUri { get; set; }

        [Required]
        public required string CreateChatCompletionUri { get; set; }

        [Required]
        public required string Model { get; set; }

        [Required]
        public required string ApiKey { get; set; }
    }
}