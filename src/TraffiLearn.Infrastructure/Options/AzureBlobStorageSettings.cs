using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Infrastructure.Options
{
    public sealed class AzureBlobStorageSettings
    {
        public const string SectionName = nameof(AzureBlobStorageSettings);

        [Required]
        [StringLength(50)]
        public string? ContainerName { get; set; }

        [Required]
        public string? ConnectionString { get; set; }
    }
}
