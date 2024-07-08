namespace TraffiLearn.Infrastructure.Options
{
    public sealed class SqlServerSettings
    {
        public const string CONFIG_KEY = "SqlServerSettings";

        public string? ConnectionString { get; set; }
    }
}
