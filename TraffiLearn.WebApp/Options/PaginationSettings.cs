namespace TraffiLearn.WebApp.Options
{
    public sealed class PaginationSettings
    {
        public const string CONFIG_KEY = "PaginationSettings";

        public int? PageSize { get; set; }
    }
}
