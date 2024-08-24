using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Questions;

namespace TraffiLearn.IntegrationTests.Topics
{
    public class TopicIntegrationTest : BaseIntegrationTest
    {
        protected readonly ApiTopicClient ApiTopicClient;
        protected readonly ApiQuestionClient ApiQuestionClient;

        public TopicIntegrationTest(
            WebApplicationFactory factory)
            : base(factory)
        {
            ApiTopicClient = new ApiTopicClient(RequestSender);

            ApiQuestionClient = new ApiQuestionClient(
                RequestSender,
                ApiTopicClient);
        }
    }
}
