using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Infrastructure.External.Blobs.Options
{
    public sealed class AzureBlobStorageSettings
    {
        public const string SectionName = nameof(AzureBlobStorageSettings);

        [Required]
        [StringLength(50)]
        public required string ContainerName { get; init; }

        [Required]
        public required string ConnectionString { get; init; }

        [Required]
        [Url]
        public required string ImagesContainerUri { get; init; }
    }
}
