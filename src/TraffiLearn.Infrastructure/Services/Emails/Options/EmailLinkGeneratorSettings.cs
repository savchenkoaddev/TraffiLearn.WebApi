using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Infrastructure.Services.Emails.Options
{
    public sealed class EmailLinkGeneratorSettings
    {
        public const string SectionName = nameof(EmailLinkGeneratorSettings);

        [Required]
        [StringLength(200)]
        public required string BaseConfirmationEndpointUri { get; init; }

        [Required]
        [StringLength(200)]
        public required string BaseConfirmChangeEmailEndpointUri { get; init; }

        [Required]
        [StringLength(200)]
        public required string BaseResetPasswordEndpointUri { get; init; }

        [Required]
        [StringLength(20)]
        public required string UserIdParameterName { get; init; }

        [Required]
        [StringLength(20)]
        public required string TokenParameterName { get; init; }

        [Required]
        [StringLength(20)]
        public required string NewEmailParameterName { get; init; }
    }
}
