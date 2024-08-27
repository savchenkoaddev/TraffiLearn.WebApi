using TraffiLearn.Application.Tickets.Commands.Update;
using TraffiLearn.IntegrationTests.Questions;
using TraffiLearn.Testing.Shared.Factories;

namespace TraffiLearn.IntegrationTests.Tickets.Commands.UpdateTicket
{
    public sealed class UpdateTicketCommandFactory
    {
        private readonly ApiQuestionClient _apiQuestionClient;

        public UpdateTicketCommandFactory(ApiQuestionClient apiQuestionClient)
        {
            _apiQuestionClient = apiQuestionClient;
        }

        public UpdateTicketCommand CreateValidCommand(
            Guid ticketId,
            List<Guid> questionIds)
        {
            return new UpdateTicketCommand(
                TicketId: ticketId,
                TicketNumber: TicketFixtureFactory.CreateNumber().Value,
                questionIds);
        }

        public async Task<UpdateTicketCommand> CreateValidCommandWithRandomQuestionIdsAsync(
            Guid ticketId)
        {
            var questionId = await _apiQuestionClient.CreateValidQuestionWithTopicAsAuthorizedAsync();

            return CreateValidCommand(
                ticketId,
                questionIds: [questionId]);
        }

        public List<UpdateTicketCommand> CreateInvalidCommandsWithRandomIds()
        {
            return [
                new UpdateTicketCommand(
                    TicketId: Guid.NewGuid(),
                    TicketNumber: -1,
                    QuestionIds: [Guid.NewGuid()]),
                    
                new UpdateTicketCommand(
                    TicketId: Guid.NewGuid(),
                    TicketNumber: null,
                    QuestionIds: [Guid.NewGuid()]),

                new UpdateTicketCommand(
                    TicketId: Guid.NewGuid(),
                    TicketNumber: 1,
                    QuestionIds: []),

                new UpdateTicketCommand(
                    TicketId: Guid.NewGuid(),
                    TicketNumber: 1,
                    QuestionIds: null),

                new UpdateTicketCommand(
                    TicketId: null,
                    TicketNumber: 1,
                    QuestionIds: [Guid.NewGuid()]),
            ];
        }
    }
}
