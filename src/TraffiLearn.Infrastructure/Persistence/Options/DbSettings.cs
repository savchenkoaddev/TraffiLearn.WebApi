using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Infrastructure.Persistence.Options
{
    public sealed class DbSettings
    {
        public const string SectionName = nameof(DbSettings);

        [Required]
        public string? ConnectionString { get; set; }
    }
}
