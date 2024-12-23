﻿using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Npgsql;
using Quartz.Spi;
using Quartz;
using Respawn;
using System.Data.Common;
using Testcontainers.Azurite;
using Testcontainers.PostgreSql;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.Users;
using TraffiLearn.Infrastructure.External.Blobs.Options;
using TraffiLearn.Infrastructure.Persistence;
using TraffiLearn.IntegrationTests.Helpers;
using TraffiLearn.WebAPI;
using TraffiLearn.Infrastructure.BackgroundJobs;
using Quartz.Impl;
using Microsoft.Extensions.Hosting;
using TraffiLearn.Application.UseCases.Users.Identity;

namespace TraffiLearn.IntegrationTests.Abstractions
{
    public sealed class WebApplicationFactory
        : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private const string AzuriteContainerImage = "mcr.microsoft.com/azure-storage/azurite:latest";

        private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase("traffilearn")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithExposedPort(5433)
            .Build();

        private readonly AzuriteContainer _azuriteContainer = new AzuriteBuilder()
            .WithImage(AzuriteContainerImage)
            .Build();

        private DbConnection _dbConnection = default!;
        private Respawner _respawner = default!;
        private UserSeeder _userSeeder = default!;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            ConfigureAppConfiguration(builder);

            builder.ConfigureTestServices(services =>
            {
                RemoveExistingDbContext(services);

                RegisterTestingDbContext(services);

                RegisterTestingMemoryCache(services);

                services.RemoveAll(typeof(BlobServiceClient));

                services.RemoveAll<QuartzHostedService>();
                services.RemoveAll<IHostedService>();

                services.AddSingleton((serviceProvider) =>
                {
                    var blobStorageSettings = serviceProvider.GetRequiredService<IOptions<AzureBlobStorageSettings>>().Value;

                    var blobServiceClient = new BlobServiceClient(_azuriteContainer.GetConnectionString());

                    var containerClient = blobServiceClient.GetBlobContainerClient(
                        blobStorageSettings.ContainerName);

                    containerClient.CreateIfNotExists();

                    return blobServiceClient;
                });
            });
        }

        private static void ConfigureAppConfiguration(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                var builtConfig = config.Build();

                var inMemorySettings = new Dictionary<string, string>
                {
                    { "QuestionsSettings:TheoryTestQuestionsCount", "20" }
                };

                config.AddInMemoryCollection(inMemorySettings!);
            });
        }

        private static void RegisterTestingMemoryCache(IServiceCollection services)
        {
            services.AddSingleton<IMemoryCache, MemoryCache>();
        }

        public async Task ResetDatabaseAsync()
        {
            await _respawner.ResetAsync(_dbConnection);
            await _userSeeder.Seed();
        }

        public async Task ResetBlobStorageAsync()
        {
            var execResult = await _azuriteContainer.ExecAsync(
                command: ["sh", "-c", "rm -rf /data/blob/*"]);
        }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
            _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());

            await _dbConnection.OpenAsync();
            await _azuriteContainer.StartAsync();

            _userSeeder = BuildUserSeeder();

            await RunMigration();

            await _userSeeder.Seed();

            await InitializeRespawner();
        }

        public new async Task DisposeAsync()
        {
            await _dbContainer.StopAsync();
            await _azuriteContainer.StopAsync();
        }

        private async Task InitializeRespawner()
        {
            _respawner = await Respawner.CreateAsync(
                _dbConnection,
                new RespawnerOptions()
                {
                    DbAdapter = DbAdapter.Postgres,
                    TablesToIgnore = ["AspNetRoles", "AspNetRoleClaims"]
                });
        }

        private async Task RunMigration()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                            .UseNpgsql(_dbConnection)
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
                options.UseNpgsql(_dbConnection);
            });
        }

        private static void RemoveExistingDbContext(IServiceCollection services)
        {
            services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
        }

        private UserSeeder BuildUserSeeder()
        {
            var scope = Services.CreateScope();

            var identityService = scope.ServiceProvider
                .GetRequiredService<IIdentityService<ApplicationUser>>();

            var userRepository = scope.ServiceProvider
                .GetRequiredService<IUserRepository>();

            var unitOfWork = scope.ServiceProvider
                .GetRequiredService<IUnitOfWork>();

            return new UserSeeder(
                identityService,
                userRepository,
                unitOfWork);
        }
    }
}
