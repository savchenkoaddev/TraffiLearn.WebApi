using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Questions.Commands.CreateQuestion;
using TraffiLearn.IntegrationTests.Questions.Commands.UpdateQuestion;
using TraffiLearn.IntegrationTests.Topics;
using TraffiLearn.IntegrationTests.Topics.Commands.CreateTopic;
using TraffiLearn.IntegrationTests.Topics.Commands.UpdateTopic;

namespace TraffiLearn.IntegrationTests.Questions
{
    public class QuestionIntegrationTest : BaseIntegrationTest
    {
        protected readonly ApiQuestionClient ApiQuestionClient;
        protected readonly ApiTopicClient ApiTopicClient;

        public QuestionIntegrationTest(
            WebApplicationFactory factory)
            : base(factory)
        {
            ApiTopicClient = new ApiTopicClient(
                RequestSender,
                new CreateTopicCommandFactory(),
                new UpdateTopicCommandFactory());

            ApiQuestionClient = new(
                RequestSender,
                new CreateQuestionCommandFactory(ApiTopicClient),
                new UpdateQuestionCommandFactory());
        }
    }
}
