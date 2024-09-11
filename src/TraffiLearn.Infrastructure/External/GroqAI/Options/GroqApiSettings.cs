using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Infrastructure.External.GroqAI.Options
{
    public sealed class GroqApiSettings
    {
        public const string SectionName = nameof(GroqApiSettings);

        [Required]
        [Url]
        public string? BaseUri { get; set; }

        [Required]
        public string? CreateChatCompletionUri { get; set; }

        [Required]
        public string? Model { get; set; }

        [Required]
        public string? ApiKey { get; set; }
    }
}