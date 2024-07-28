using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace TraffiLearn.Infrastructure.Options
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
