using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Infrastructure.Services.Emails.Options
{
    public sealed class SmtpClientSettings
    {
        public const string SectionName = nameof(SmtpClientSettings);

        [Required]
        [Range(1, int.MaxValue)]
        public int Port { get; init; }

        [Required]
        public bool EnableSsl { get; init; }

        [Required]
        [StringLength(100)]
        public required string Host { get; init; }

        [StringLength(100)]
        public string? Username { get; init; }

        [StringLength(100)]
        public string? Password { get; init; }
    }
}
