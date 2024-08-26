using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Application.Tickets.Commands.Create;
using TraffiLearn.Application.Tickets.DTO;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Helpers;
using TraffiLearn.IntegrationTests.Questions;
using TraffiLearn.IntegrationTests.Tickets.CreateTicket;

namespace TraffiLearn.IntegrationTests.Tickets
{
    public sealed class ApiTicketClient
    {
        private readonly RequestSender _requestSender;
        private readonly ApiQuestionClient _apiQuestionClient;
        private readonly CreateTicketCommandFactory _createTicketCommandFactory;

        public ApiTicketClient(
            RequestSender requestSender,
            ApiQuestionClient apiQuestionClient,
            CreateTicketCommandFactory createTicketCommandFactory)
        {
            _requestSender = requestSender;
            _apiQuestionClient = apiQuestionClient;
            _createTicketCommandFactory = createTicketCommandFactory;
        }

        public async Task<Guid> CreateValidTicketWithQuestionIdsAsync(
            Role? createdWithRole = null)
        {
            var questionId = await _apiQuestionClient.CreateValidQuestionWithTopicAsAuthorizedAsync();

            var command = _createTicketCommandFactory.CreateValidCommand(
                questionIds: [questionId]);

            return await CreateTicketAsync(
                command,
                createdWithRole);
        }

        public async Task<Guid> CreateValidTicketWithQuestionIdsAsAuthorizedAsync()
        {
            var questionId = await _apiQuestionClient.CreateValidQuestionWithTopicAsAuthorizedAsync();

            var command = _createTicketCommandFactory.CreateValidCommand(
                questionIds: [questionId]);

            return await CreateTicketAsync(
                command,
                createdWithRole: Role.Owner);
        }

        public async Task<Guid> CreateValidTicketAsync(
            List<Guid> questionIds,
            Role? createdWithRole = null)
        {
            var command = _createTicketCommandFactory.CreateValidCommand(questionIds);

            return await CreateTicketAsync(
                command,
                createdWithRole);
        }

        private async Task<Guid> CreateTicketAsync(
            CreateTicketCommand command,
            Role? createdWithRole = null)
        {
            return await _requestSender
                .SendAndGetJsonAsync<CreateTicketCommand, Guid>(
                    method: HttpMethod.Post,
                    requestUri: TicketEndpointRoutes.CreateTicketRoute,
                    value: command,
                    sentWithRole: createdWithRole);
        }

        public async Task<HttpResponseMessage> SendCreateTicketRequestWithQuestionsAsync(
            Role? sentFromRole = null)
        {
            var command = await _createTicketCommandFactory
                .CreateValidCommandWithQuestionIdsAsync();

            return await _requestSender.SendJsonAsync(
                HttpMethod.Post,
                requestUri: TicketEndpointRoutes.CreateTicketRoute,
                value: command,
                sentFromRole);
        }

        public async Task<HttpResponseMessage> SendCreateTicketRequestAsync(
            List<Guid> questionIds,
            Role? sentFromRole = null)
        {
            var command = _createTicketCommandFactory
                .CreateValidCommand(questionIds);

            return await _requestSender.SendJsonAsync(
                HttpMethod.Post,
                requestUri: TicketEndpointRoutes.CreateTicketRoute,
                value: command,
                sentFromRole);
        }

        public Task<IEnumerable<TicketResponse>> GetAllTicketsAsync(
            Role? getWithRole = null)
        {
            return _requestSender.GetFromJsonAsync<IEnumerable<TicketResponse>>(
                requestUri: TicketEndpointRoutes.GetAllTicketsRoute,
                getWithRole);
        }

        public Task<IEnumerable<TicketResponse>> GetAllTicketsAsAuthorizedUserAsync()
        {
            return GetAllTicketsAsync(
                getWithRole: Role.Owner);
        }

        public Task<IEnumerable<QuestionResponse>> GetTicketQuestionsAsync(
            Guid ticketId,
            Role? getWithRole = null)
        {
            return _requestSender.GetFromJsonAsync<IEnumerable<QuestionResponse>>(
                requestUri: TicketEndpointRoutes.GetTicketQuestionsRoute(ticketId),
                getWithRole: getWithRole);
        }

        public Task<IEnumerable<QuestionResponse>> GetTicketQuestionsAsAuthorizedUserAsync(
            Guid ticketId)
        {
            return GetTicketQuestionsAsync(
                ticketId,
                getWithRole: Role.Owner);
        }

        public Task<HttpResponseMessage> SendDeleteTicketRequestAsync(
            Guid ticketId,
            Role? sentFromRole = null)
        {
            return _requestSender.DeleteAsync(
                requestUri: TicketEndpointRoutes.DeleteTicketRoute(ticketId),
                deletedWithRole: sentFromRole);
        }

        public async Task DeleteTicketAsync(
            Guid ticketId,
            Role? deletedWithRole = null)
        {
            var response = await SendDeleteTicketRequestAsync(
                ticketId,
                sentFromRole: deletedWithRole);

            response.EnsureSuccessStatusCode();
        }
    }
}
