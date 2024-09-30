using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Infrastructure.External.Emails.Options
{
    public sealed class SmtpClientSettings
    {
        public const string SectionName = nameof(SmtpClientSettings);

        [Required]
        public int Port { get; set; }

        [Required]
        [StringLength(100)]
        public string Host { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }
    }
}
