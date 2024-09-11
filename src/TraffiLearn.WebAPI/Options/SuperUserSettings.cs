using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.WebAPI.Options
{
    public sealed class SuperUserSettings
    {
        public const string SectionName = nameof(SuperUserSettings);

        [Required]
        public string? Username { get; set; }

        [EmailAddress]
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
