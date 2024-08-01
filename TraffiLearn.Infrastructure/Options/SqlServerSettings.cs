namespace TraffiLearn.Infrastructure.Options
{
    public sealed class SqlServerSettings
    {
        public const string SectionName = nameof(SqlServerSettings);

        public string? ConnectionString { get; set; }
    }
}
