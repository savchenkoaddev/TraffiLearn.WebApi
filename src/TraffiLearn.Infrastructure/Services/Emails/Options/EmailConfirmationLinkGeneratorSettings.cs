using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Infrastructure.Services.Emails.Options
{
    public sealed class EmailConfirmationLinkGeneratorSettings
    {
        public const string SectionName = nameof(EmailConfirmationLinkGeneratorSettings);

        [Required]
        [StringLength(200)]
        public string? BaseConfirmationEndpointUri { get; set; }

        [Required]
        [StringLength(20)]
        public string? UserIdParameterName { get; set; }

        [Required]
        [StringLength(20)]
        public string? TokenParameterName { get; set; }
    }
}
