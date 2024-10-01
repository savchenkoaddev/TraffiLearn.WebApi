using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Infrastructure.Services.Emails.Options
{
    public sealed class SmtpClientSettings
    {
        public const string SectionName = nameof(SmtpClientSettings);

        [Required]
        [Range(1, int.MaxValue)]
        public int Port { get; set; }

        [Required]
        public bool EnableSsl { get; set; }

        [Required]
        [StringLength(100)]
        public string? Host { get; set; }

        [StringLength(100)]
        public string? Username { get; set; }

        [StringLength(100)]
        public string? Password { get; set; }
    }
}
