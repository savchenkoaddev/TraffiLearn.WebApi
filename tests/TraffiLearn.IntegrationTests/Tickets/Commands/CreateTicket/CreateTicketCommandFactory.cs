using TraffiLearn.Application.Tickets.Commands.Create;
using TraffiLearn.IntegrationTests.Questions;
using TraffiLearn.Testing.Shared.Factories;

namespace TraffiLearn.IntegrationTests.Tickets.Commands.CreateTicket
{
    public sealed class CreateTicketCommandFactory
    {
        private readonly ApiQuestionClient _apiQuestionClient;

        public CreateTicketCommandFactory(ApiQuestionClient apiQuestionClient)
        {
            _apiQuestionClient = apiQuestionClient;
        }

        public CreateTicketCommand CreateValidCommand(
            List<Guid> questionIds)
        {
            return new CreateTicketCommand(
                TicketNumber: TicketFixtureFactory.CreateNumber().Value,
                questionIds);
        }

        public async Task<CreateTicketCommand> CreateValidCommandWithQuestionIdsAsync()
        {
            var questionId = await _apiQuestionClient.CreateValidQuestionWithTopicAsAuthorizedAsync();

            return CreateValidCommand(
                questionIds: [questionId]);
        }

        public List<CreateTicketCommand> CreateInvalidCommands()
        {
            return [
                new CreateTicketCommand(
                    TicketNumber: -1,
                    QuestionIds: [Guid.NewGuid()]),

                new CreateTicketCommand(
                    TicketNumber: null,
                    QuestionIds: [Guid.NewGuid()]),

                new CreateTicketCommand(
                    TicketNumber: 1,
                    QuestionIds: []),

                new CreateTicketCommand(
                    TicketNumber: 1,
                    QuestionIds: null),
            ];
        }
    }
}
