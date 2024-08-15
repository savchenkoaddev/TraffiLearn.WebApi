using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Infrastructure.Options
{
    public sealed class SqlServerSettings
    {
        public const string SectionName = nameof(SqlServerSettings);

        [Required]
        public string? ConnectionString { get; set; }
    }
}
