using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using TraffiLearn.IntegrationTests.Auth;
using TraffiLearn.IntegrationTests.Helpers;

namespace TraffiLearn.IntegrationTests.Abstractions
{
    [Collection(Constants.CollectionName)]
    public abstract class BaseIntegrationTest : IAsyncLifetime
    {
        private readonly Func<Task> ResetDatabase;

        protected BaseIntegrationTest(
            WebApplicationFactory factory)
        {
            ResetDatabase = factory.ResetDatabaseAsync;
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

        public Task DisposeAsync() => ResetDatabase();
    }
}
