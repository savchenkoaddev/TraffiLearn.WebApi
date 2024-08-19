using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace TraffiLearn.IntegrationTests
{
    [Collection(Constants.CollectionName)]
    public abstract class BaseIntegrationTest : IAsyncLifetime
    {
        private readonly IServiceScope _scope;
        protected readonly ISender Sender;
        private readonly Func<Task> ResetDatabase;

        protected BaseIntegrationTest(
            IntegrationTestWebAppFactory factory)
        {
            _scope = factory.Services.CreateScope();

            Sender = _scope.ServiceProvider.GetRequiredService<ISender>();

            ResetDatabase = factory.ResetDatabaseAsync;
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public Task DisposeAsync() => ResetDatabase();
    }
}
