using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Infrastructure.Options
{
    public sealed class EmailConfirmationLinkGeneratorSettings
    {
        public const string SectionName = nameof(EmailConfirmationLinkGeneratorSettings);

        [Required]
        [StringLength(200)]
        public string? BaseConfirmationEndpointUri { get; set; }

        [StringLength(20)]
        public string? UserIdParameterName { get; set; } = "userId";

        [StringLength(20)]
        public string? TokenParameterName { get; set; } = "token";
    }
}
