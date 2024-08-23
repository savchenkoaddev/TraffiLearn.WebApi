using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;

namespace TraffiLearn.IntegrationTests.Topics.Queries.GetRandomTopicWithQuestions
{
    public sealed class GetRandomTopicWithQuestionsTests : TopicIntegrationTest
    {
        public GetRandomTopicWithQuestionsTests(
            WebApplicationFactory factory)
            : base(factory)
        { }

        [Fact]
        public async Task GetRandomTopicWithQuestions_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await RequestSender.GetAsync(
                requestUri: TopicEndpointRoutes.GetAllSortedTopicsByNumberRoute);

            response.AssertUnauthorizedStatusCode();
        }
    }
}
