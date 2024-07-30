namespace TraffiLearn.Infrastructure.Options
{
    public sealed class AzureBlobStorageSettings
    {
        public string? ContainerName { get; set; }

        public string? ConnectionString { get; set; }
    }
}
