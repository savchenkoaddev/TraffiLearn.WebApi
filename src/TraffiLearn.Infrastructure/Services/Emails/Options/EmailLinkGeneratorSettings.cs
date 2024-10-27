using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Infrastructure.Services.Emails.Options
{
    public sealed class EmailLinkGeneratorSettings
    {
        public const string SectionName = nameof(EmailLinkGeneratorSettings);

        [Required]
        [StringLength(200)]
        public string? BaseConfirmationEndpointUri { get; set; }

        [Required]
        [StringLength(200)]
        public string? BaseConfirmChangeEmailEndpointUri { get; set; }

        [Required]
        [StringLength(200)]
        public string? BaseResetPasswordEndpointUri { get; set; }

        [Required]
        [StringLength(20)]
        public string? UserIdParameterName { get; set; }

        [Required]
        [StringLength(20)]
        public string? TokenParameterName { get; set; }

        [Required]
        [StringLength(20)]
        public string? NewEmailParameterName { get; set; }
    }
}
