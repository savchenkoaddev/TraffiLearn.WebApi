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

            RequestSender = new RequestSender(
                HttpClient, 
                Authenticator);
        }

        protected HttpClient HttpClient { get; private init; }

        protected Authenticator Authenticator { get; private init; }

        protected RequestSender RequestSender { get; private init; }

        public Task InitializeAsync() => Task.CompletedTask;

        public Task DisposeAsync() => ResetDatabase();
    }
}
