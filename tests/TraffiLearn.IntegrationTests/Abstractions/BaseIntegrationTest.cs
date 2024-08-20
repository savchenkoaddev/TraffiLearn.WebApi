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
        }

        protected HttpClient HttpClient { get; init; }

        public Task InitializeAsync() => Task.CompletedTask;

        public Task DisposeAsync() => ResetDatabase();
    }
}
