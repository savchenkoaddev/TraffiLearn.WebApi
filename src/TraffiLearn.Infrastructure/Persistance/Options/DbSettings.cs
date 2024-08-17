using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Infrastructure.Persistance.Options
{
    public sealed class DbSettings
    {
        public const string SectionName = nameof(DbSettings);

        [Required]
        public string? ConnectionString { get; set; }
    }
}
