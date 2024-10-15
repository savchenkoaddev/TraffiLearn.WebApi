using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Application.Images.Options
{
    public sealed class ImageSettings
    {
        public const string SectionName = nameof(ImageSettings);

        [Required]
        public required HashSet<string> AllowedContentTypes { get; init; }
    }
}
