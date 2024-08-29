using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;
using TraffiLearn.Application.Questions.Commands.Create;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Application.Tickets.DTO;
using TraffiLearn.Application.Topics.DTO;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Helpers;
using TraffiLearn.IntegrationTests.Questions.CreateQuestion;

namespace TraffiLearn.IntegrationTests.Questions
{
    public sealed class ApiQuestionClient
    {
        private readonly RequestSender _requestSender;
        private readonly CreateQuestionCommandFactory _createQuestionCommandFactory;

        public ApiQuestionClient(
            RequestSender requestSender,
            CreateQuestionCommandFactory createQuestionCommandFactory)
        {
            _requestSender = requestSender;
            _createQuestionCommandFactory = createQuestionCommandFactory;
        }

        public async Task<HttpResponseMessage> SendValidCreateQuestionRequestWithTopicAsync(
            IFormFile? image = null,
            Role? sentFromRole = null)
        {
            var command = await _createQuestionCommandFactory.CreateValidCommandWithTopicAsync(
                image);

            return await SendCreateQuestionRequestAsync(
                command,
                image,
                createdWithRole: sentFromRole);
        }

        public Task<Guid> CreateValidQuestionWithTopicAsAuthorizedAsync(
            IFormFile? image = null)
        {
            return CreateValidQuestionWithTopicAsync(
                image,
                createdWithRole: Role.Owner);
        }

        public async Task<Guid> CreateValidQuestionWithTopicAsync(
            IFormFile? image = null,
            Role? createdWithRole = null)
        {
            var command = await _createQuestionCommandFactory.CreateValidCommandWithTopicAsync(
                image);

            return await CreateQuestionAsync(
                command,
                image,
                createdWithRole);
        }

        public async Task<HttpResponseMessage> SendValidCreateQuestionRequestAsync(
            List<Guid> topicIds,
            IFormFile? image = null,
            Role? sentFromRole = null)
        {
            var command = _createQuestionCommandFactory.CreateValidCommand(
                topicIds,
                image);

            return await SendCreateQuestionRequestAsync(
                command,
                image,
                createdWithRole: sentFromRole);
        }

        public Task<Guid> CreateValidQuestionAsAuthorizedAsync(
            List<Guid> topicIds,
            IFormFile? image = null)
        {
            return CreateValidQuestionAsync(
                topicIds,
                image,
                createdWithRole: Role.Owner);
        }

        public async Task<Guid> CreateValidQuestionAsync(
            List<Guid> topicIds,
            IFormFile? image = null,
            Role? createdWithRole = null)
        {
            var request = _createQuestionCommandFactory.CreateValidCommand(
                topicIds,
                image);

            var questionId = await CreateQuestionAsync(
                command: request,
                image,
                createdWithRole: createdWithRole);

            return questionId;
        }

        private async Task<Guid> CreateQuestionAsync(
            CreateQuestionCommand command,
            IFormFile? image,
            Role? createdWithRole = null)
        {
            var response = await SendCreateQuestionRequestAsync(
                command,
                image,
                createdWithRole);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Guid>();
        }

        private async Task<HttpResponseMessage> SendCreateQuestionRequestAsync(
            CreateQuestionCommand command,
            IFormFile? image,
            Role? createdWithRole = null)
        {
            return await _requestSender.SendMultipartFormDataWithJsonAndFileRequest(
                method: HttpMethod.Post,
                requestUri: QuestionEndpointRoutes.CreateQuestionRoute,
                value: command,
                file: image,
                sentFromRole: createdWithRole);
        }

        public Task<IEnumerable<QuestionResponse>> GetAllQuestionsAsAuthorizedAsync()
        {
            return GetAllQuestionsAsync(
                getWithRole: Role.Owner);
        }

        public Task<IEnumerable<QuestionResponse>> GetAllQuestionsAsync(
            Role? getWithRole = null)
        {
            return _requestSender.GetFromJsonAsync<IEnumerable<QuestionResponse>>(
                requestUri: QuestionEndpointRoutes.GetAllQuestionsRoute,
                getWithRole: getWithRole);
        }

        public Task<IEnumerable<TopicResponse>> GetQuestionTopicsAsync(
            Guid questionId,
            Role? getWithRole = null)
        {
            return _requestSender.GetFromJsonAsync<IEnumerable<TopicResponse>>(
                requestUri: QuestionEndpointRoutes.GetQuestionTopicsRoute(questionId),
                getWithRole: getWithRole);
        }

        public Task<IEnumerable<TopicResponse>> GetQuestionTopicsAsAuthorizedUserAsync(
            Guid questionId)
        {
            return _requestSender.GetFromJsonAsync<IEnumerable<TopicResponse>>(
                requestUri: QuestionEndpointRoutes.GetQuestionTopicsRoute(questionId),
                getWithRole: Role.Owner);
        }

        public Task<IEnumerable<TicketResponse>> GetQuestionTicketsAsync(
            Guid questionId,
            Role? getWithRole = null)
        {
            return _requestSender.GetFromJsonAsync<IEnumerable<TicketResponse>>(
                requestUri: QuestionEndpointRoutes.GetQuestionTicketsRoute(questionId),
                getWithRole);
        }

        public Task<IEnumerable<TicketResponse>> GetQuestionTicketsAsAuthorizedUserAsync(
            Guid questionId)
        {
            return GetQuestionTicketsAsync(
                questionId,
                getWithRole: Role.Owner);
        }
    }
}
