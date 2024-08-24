using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Questions;
using TraffiLearn.IntegrationTests.Tickets.CreateTicket;
using TraffiLearn.IntegrationTests.Topics;

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
            var apiTopicClient = new ApiTopicClient(RequestSender);

            ApiQuestionClient = new ApiQuestionClient(
                RequestSender, 
                apiTopicClient);

            ApiTicketClient = new ApiTicketClient(
                RequestSender,
                ApiQuestionClient,
                new CreateTicketCommandFactory(ApiQuestionClient));
        }
    }
}
