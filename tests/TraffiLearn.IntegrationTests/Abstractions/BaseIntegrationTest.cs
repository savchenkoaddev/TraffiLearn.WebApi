using Azure.Storage.Blobs;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using TraffiLearn.IntegrationTests.Auth;
using TraffiLearn.IntegrationTests.Helpers;

namespace TraffiLearn.IntegrationTests.Abstractions
{
    [Collection(Constants.CollectionName)]
    public abstract class BaseIntegrationTest : IAsyncLifetime
    {
        private readonly Func<Task> _resetDatabase;
        private readonly Func<Task> _resetBlobStorage;

        protected BaseIntegrationTest(
            WebApplicationFactory factory)
        {
            _resetDatabase = factory.ResetDatabaseAsync;
            _resetBlobStorage = factory.ResetBlobStorageAsync;

            HttpClient = factory.CreateClient();

            Authenticator = new Authenticator(HttpClient);

            TestingMemoryCache = factory.Services.GetRequiredService<IMemoryCache>();

            RequestSender = new RequestSender(
                HttpClient,
                Authenticator,
                TestingMemoryCache);
        }

        protected HttpClient HttpClient { get; private init; }

        protected Authenticator Authenticator { get; private init; }

        protected RequestSender RequestSender { get; private init; }

        protected IMemoryCache TestingMemoryCache { get; private init; }

        public Task InitializeAsync() => Task.CompletedTask;

        public async Task DisposeAsync()
        {
            await _resetDatabase();
            await _resetBlobStorage();
        }
    }
}
