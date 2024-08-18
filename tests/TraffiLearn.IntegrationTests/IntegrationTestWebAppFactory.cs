using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.Azurite;
using Testcontainers.MsSql;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Infrastructure.External.Blobs.Options;
using TraffiLearn.Infrastructure.Persistence;
using TraffiLearn.WebAPI;

namespace TraffiLearn.IntegrationTests
{
    public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder().Build();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var dbDescriptor = services.SingleOrDefault(
                    s => s.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (dbDescriptor is not null)
                {
                    services.Remove(dbDescriptor); 
                }

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlServer(_msSqlContainer.GetConnectionString());
                });
            });
        }

        public Task InitializeAsync()
        {
            return _msSqlContainer.StartAsync();
        }

        public new Task DisposeAsync()
        {
            return _msSqlContainer.StopAsync();
        }
    }
}
