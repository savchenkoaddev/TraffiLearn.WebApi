namespace TraffiLearn.Infrastructure.Options
{
    public sealed class AzureBlobStorageSettings
    {
        public const string CONFIG_KEY = "AzureBlobStorageSettings";

        public string? ContainerName { get; set; }

        public string? ConnectionString { get; set; }
    }
}
