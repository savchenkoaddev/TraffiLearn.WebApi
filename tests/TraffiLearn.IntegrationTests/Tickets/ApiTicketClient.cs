﻿using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Application.Tickets.Commands.Create;
using TraffiLearn.Application.Tickets.Commands.Update;
using TraffiLearn.Application.Tickets.DTO;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Helpers;
using TraffiLearn.IntegrationTests.Questions;
using TraffiLearn.IntegrationTests.Tickets.Commands.CreateTicket;
using TraffiLearn.IntegrationTests.Tickets.Commands.UpdateTicket;
using TraffiLearn.Testing.Shared.Factories;

namespace TraffiLearn.IntegrationTests.Tickets
{
    public sealed class ApiTicketClient
    {
        private readonly RequestSender _requestSender;
        private readonly ApiQuestionClient _apiQuestionClient;
        private readonly CreateTicketCommandFactory _createTicketCommandFactory;
        private readonly UpdateTicketCommandFactory _updateTicketCommandFactory;

        public ApiTicketClient(
            RequestSender requestSender,
            ApiQuestionClient apiQuestionClient,
            CreateTicketCommandFactory createTicketCommandFactory,
            UpdateTicketCommandFactory updateTicketCommandFactory)
        {
            _requestSender = requestSender;
            _apiQuestionClient = apiQuestionClient;
            _createTicketCommandFactory = createTicketCommandFactory;
            _updateTicketCommandFactory = updateTicketCommandFactory;
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

        public Task<Guid> CreateValidTicketAsAuthorizedAsync(
            List<Guid> questionIds)
        {
            return CreateValidTicketAsync(
                questionIds: questionIds,
                createdWithRole: Role.Owner);
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

        public async Task<HttpResponseMessage> SendValidUpdateTicketRequestWithRandomQuestionIdsAsync(
            Guid ticketId,
            Role? updatedWithRole = null)
        {
            var command = await _updateTicketCommandFactory
                .CreateValidCommandWithRandomQuestionIdsAsync(ticketId);

            return await SendUpdateTicketRequestAsync(
                command,
                sentFromRole: updatedWithRole);
        }

        public async Task<HttpResponseMessage> SendValidUpdateTicketRequestAsync(
            Guid ticketId,
            List<Guid> questionIds,
            Role? sentFromRole = null)
        {
            var command = _updateTicketCommandFactory
                .CreateValidCommand(
                    ticketId,
                    questionIds);

            return await SendUpdateTicketRequestAsync(
                command,
                sentFromRole: sentFromRole);
        }

        public Task<HttpResponseMessage> SendUpdateTicketRequestAsync(
            UpdateTicketCommand request,
            Role? sentFromRole = null)
        {
            return _requestSender.SendJsonAsync(
                method: HttpMethod.Put,
                requestUri: TicketEndpointRoutes.UpdateTicketRoute,
                value: request,
                sentFromRole: sentFromRole);
        }

        public Task<HttpResponseMessage> SendAddQuestionToTicketRequestAsync(
            Guid questionId,
            Guid ticketId,
            Role? sentFromRole = null)
        {
            return _requestSender.PutAsync(
                requestUri: TicketEndpointRoutes.AddQuestionToTicketRoute(
                    questionId: questionId,
                    ticketId: ticketId),
                putWithRole: sentFromRole);
        }

        public Task<HttpResponseMessage> SendRemoveQuestionFromTicketRequestAsync(
            Guid questionId,
            Guid ticketId,
            Role? sentFromRole = null)
        {
            return _requestSender.PutAsync(
                requestUri: TicketEndpointRoutes.RemoveQuestionFromTicketRoute(
                    questionId: questionId,
                    ticketId: ticketId),
                putWithRole: sentFromRole);
        }
    }
}
