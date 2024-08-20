using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Respawn;
using System.Data.Common;
using Testcontainers.MsSql;
using TraffiLearn.Infrastructure.Persistence;
using TraffiLearn.WebAPI;

namespace TraffiLearn.IntegrationTests.Abstractions
{
    public sealed class WebApplicationFactory
        : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
            .Build();

        private DbConnection _dbConnection = default!;
        private Respawner _respawner = default!;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                RemoveExistingDbContext(services);

                RegisterTestingDbContext(services);
            });
        }

        public async Task ResetDatabaseAsync()
        {
            await _respawner.ResetAsync(_dbConnection);
        }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
            _dbConnection = new SqlConnection(_dbContainer.GetConnectionString());

            await _dbConnection.OpenAsync();
            await RunMigration();

            await InitializeRespawner();
        }

        public new Task DisposeAsync()
        {
            return _dbContainer.StopAsync();
        }

        private async Task InitializeRespawner()
        {
            _respawner = await Respawner.CreateAsync(
                _dbConnection,
                new RespawnerOptions()
                {
                    DbAdapter = DbAdapter.SqlServer
                });
        }

        private async Task RunMigration()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                            .UseSqlServer(_dbConnection)
                            .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                await dbContext.Database.MigrateAsync();
            }
        }

        private void RegisterTestingDbContext(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(_dbConnection);
            });
        }

        private static void RemoveExistingDbContext(IServiceCollection services)
        {
            services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
        }
    }
}
