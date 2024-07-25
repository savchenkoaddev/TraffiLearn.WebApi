using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using TraffiLearn.Infrastructure.Options;

namespace TraffiLearn.Infrastructure.OptionsSetup
{
    internal sealed class SqlServerSettingsSetup : IConfigureOptions<SqlServerSettings>
    {
        private const string SectionName = "SqlServerSettings";
        private readonly IConfiguration _configuration;

        public SqlServerSettingsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(SqlServerSettings options)
        {
            _configuration
                .GetSection(SectionName)
                .Bind(options);
        }
    }
}
