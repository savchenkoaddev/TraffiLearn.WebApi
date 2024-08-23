using TraffiLearn.IntegrationTests.Abstractions;

namespace TraffiLearn.IntegrationTests
{
    [CollectionDefinition(Constants.CollectionName)]
    public sealed class SharedTestCollection
        : ICollectionFixture<WebApplicationFactory>
    { }
}
