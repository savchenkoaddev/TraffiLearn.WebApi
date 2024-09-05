using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Questions;
using TraffiLearn.IntegrationTests.Questions.Commands.CreateQuestion;
using TraffiLearn.IntegrationTests.Questions.Commands.UpdateQuestion;
using TraffiLearn.IntegrationTests.Questions.Queries.GetRandomQuestions;
using TraffiLearn.IntegrationTests.Tickets.Commands.CreateTicket;
using TraffiLearn.IntegrationTests.Tickets.Commands.UpdateTicket;
using TraffiLearn.IntegrationTests.Topics;
using TraffiLearn.IntegrationTests.Topics.Commands.CreateTopic;
using TraffiLearn.IntegrationTests.Topics.Commands.UpdateTopic;

namespace TraffiLearn.IntegrationTests.Tickets
{
    public class TicketIntegrationTest : BaseIntegrationTest
    {
        protected readonly ApiTicketClient ApiTicketClient;
        protected readonly ApiQuestionClient ApiQuestionClient;

        public TicketIntegrationTest(
            WebApplicationFactory factory)
            : base(factory)
        {
            var apiTopicClient = new ApiTopicClient(
                RequestSender,
                new CreateTopicCommandFactory(),
                new UpdateTopicCommandFactory());

            ApiQuestionClient = new ApiQuestionClient(
                RequestSender,
                new CreateQuestionCommandFactory(apiTopicClient),
                new UpdateQuestionCommandFactory(),
                new GetRandomQuestionsQueryFactory());

            ApiTicketClient = new ApiTicketClient(
                RequestSender,
                ApiQuestionClient,
                new CreateTicketCommandFactory(ApiQuestionClient),
                new UpdateTicketCommandFactory(ApiQuestionClient));
        }
    }
}
