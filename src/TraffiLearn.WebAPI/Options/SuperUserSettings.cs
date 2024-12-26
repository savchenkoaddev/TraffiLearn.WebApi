using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.WebAPI.Options
{
    public sealed class SuperUserSettings
    {
        public const string SectionName = nameof(SuperUserSettings);

        [Required]
        public required string Username { get; init; }

        [EmailAddress]
        [Required]
        public required string Email { get; init; }

        [Required]
        public required string Password { get; init; }
    }
}
