using TraffiLearn.IntegrationTests.Abstractions;

namespace TraffiLearn.IntegrationTests.Topics
{
    public class TopicIntegrationTest : BaseIntegrationTest
    {
        protected readonly ApiTopicClient ApiTopicClient;

        public TopicIntegrationTest(
            WebApplicationFactory factory)
            : base(factory)
        {
            ApiTopicClient = new ApiTopicClient(RequestSender);
        }
    }
}
