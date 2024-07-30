using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace TraffiLearn.Infrastructure.Options
{
    internal sealed class AzureBlobStorageSettingsSetup : IConfigureOptions<AzureBlobStorageSettings>
    {
        private const string SectionName = "AzureBlobStorageSettings";
        private readonly IConfiguration _configuration;

        public AzureBlobStorageSettingsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(AzureBlobStorageSettings options)
        {
            _configuration
                .GetSection(SectionName)
                .Bind(options);
        }
    }
}
