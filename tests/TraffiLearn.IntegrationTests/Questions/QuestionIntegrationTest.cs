using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Questions.Commands.CreateQuestion;
using TraffiLearn.IntegrationTests.Questions.Commands.UpdateQuestion;
using TraffiLearn.IntegrationTests.Questions.Queries.GetRandomQuestions;
using TraffiLearn.IntegrationTests.Tickets;
using TraffiLearn.IntegrationTests.Tickets.Commands.CreateTicket;
using TraffiLearn.IntegrationTests.Tickets.Commands.UpdateTicket;
using TraffiLearn.IntegrationTests.Topics;
using TraffiLearn.IntegrationTests.Topics.Commands.CreateTopic;
using TraffiLearn.IntegrationTests.Topics.Commands.UpdateTopic;

namespace TraffiLearn.IntegrationTests.Questions
{
    public class QuestionIntegrationTest : BaseIntegrationTest
    {
        protected readonly ApiQuestionClient ApiQuestionClient;
        protected readonly ApiTopicClient ApiTopicClient;
        protected readonly ApiTicketClient ApiTicketClient;

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
                new UpdateQuestionCommandFactory(),
                new GetRandomQuestionsQueryFactory());

            ApiTicketClient = new(
                RequestSender,
                ApiQuestionClient,
                new CreateTicketCommandFactory(ApiQuestionClient),
                new UpdateTicketCommandFactory(ApiQuestionClient));
        }
    }
}
