using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Questions;
using TraffiLearn.IntegrationTests.Topics.Commands.CreateTopic;
using TraffiLearn.IntegrationTests.Topics.Commands.UpdateTopic;

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
            ApiTopicClient = new ApiTopicClient(
                RequestSender,
                new CreateTopicCommandFactory(),
                new UpdateTopicCommandFactory());

            ApiQuestionClient = new ApiQuestionClient(
                RequestSender,
                ApiTopicClient);
        }
    }
}
