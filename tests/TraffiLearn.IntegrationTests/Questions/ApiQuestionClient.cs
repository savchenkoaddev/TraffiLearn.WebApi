using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;
using TraffiLearn.Application.Questions.Commands.Create;
using TraffiLearn.Application.Questions.Commands.Update;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Application.Tickets.DTO;
using TraffiLearn.Application.Topics.DTO;
using TraffiLearn.Domain.Aggregates.Users.Roles;
using TraffiLearn.IntegrationTests.Helpers;
using TraffiLearn.IntegrationTests.Questions.Commands.CreateQuestion;
using TraffiLearn.IntegrationTests.Questions.Commands.UpdateQuestion;
using TraffiLearn.IntegrationTests.Questions.Queries.GetRandomQuestions;

namespace TraffiLearn.IntegrationTests.Questions
{
    public sealed class ApiQuestionClient
    {
        private readonly RequestSender _requestSender;
        private readonly CreateQuestionCommandFactory _createQuestionCommandFactory;
        private readonly UpdateQuestionCommandFactory _updateQuestionCommandFactory;
        private readonly GetRandomQuestionsQueryFactory _getRandomQuestionsQueryFactory;

        public ApiQuestionClient(
            RequestSender requestSender,
            CreateQuestionCommandFactory createQuestionCommandFactory,
            UpdateQuestionCommandFactory updateQuestionCommandFactory,
            GetRandomQuestionsQueryFactory getRandomQuestionsQueryFactory)
        {
            _requestSender = requestSender;
            _createQuestionCommandFactory = createQuestionCommandFactory;
            _updateQuestionCommandFactory = updateQuestionCommandFactory;
            _getRandomQuestionsQueryFactory = getRandomQuestionsQueryFactory;
        }

        public async Task<HttpResponseMessage> SendValidCreateQuestionRequestWithTopicAsync(
            IFormFile? image = null,
            Role? sentFromRole = null)
        {
            var command = await _createQuestionCommandFactory.CreateValidCommandWithTopicAsync(
                image);

            return await SendCreateQuestionRequestAsync(
                command,
                sentFromRole: sentFromRole);
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
                sentFromRole: sentFromRole);
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
                createdWithRole: createdWithRole);

            return questionId;
        }

        private async Task<Guid> CreateQuestionAsync(
            CreateQuestionCommand command,
            Role? createdWithRole = null)
        {
            var response = await SendCreateQuestionRequestAsync(
                command,
                createdWithRole);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Guid>();
        }

        public Task<HttpResponseMessage> SendCreateQuestionRequestAsync(
            CreateQuestionCommand command,
            Role? sentFromRole = null)
        {
            return _requestSender.SendMultipartFormDataWithJsonAndFileRequest(
                method: HttpMethod.Post,
                requestUri: QuestionEndpointRoutes.CreateQuestionRoute,
                value: command,
                file: command.Image,
                sentFromRole: sentFromRole);
        }

        public Task<HttpResponseMessage> SendDeleteQuestionRequestAsync(
            Guid questionId,
            Role? sentFromRole = null)
        {
            return _requestSender.DeleteAsync(
                requestUri: QuestionEndpointRoutes.DeleteQuestionRoute(questionId),
                deletedWithRole: sentFromRole);
        }

        public Task<HttpResponseMessage> SendUpdateQuestionRequestAsync(
            UpdateQuestionCommand request,
            Role? sentFromRole = null)
        {
            return _requestSender.SendMultipartFormDataWithJsonAndFileRequest(
                method: HttpMethod.Put,
                requestUri: QuestionEndpointRoutes.UpdateQuestionRoute,
                value: request,
                file: request.Image,
                sentFromRole: sentFromRole);
        }

        public Task<HttpResponseMessage> SendValidUpdateQuestionRequestAsync(
            Guid questionId,
            List<Guid>? topicIds,
            IFormFile? image = null,
            bool? removeOldImageIfNewImageMissing = true,
            Role? sentFromRole = null)
        {
            var validCommand = _updateQuestionCommandFactory.CreateValidCommand(
                questionId,
                topicIds,
                image,
                removeOldImageIfNewImageMissing);

            return SendUpdateQuestionRequestAsync(
                request: validCommand,
                sentFromRole: sentFromRole);
        }

        public async Task<IEnumerable<QuestionResponse>> GetPaginatedQuestionsAsAuthorizedAsync()
        {
            return (await GetPaginatedQuestionsAsync(
                getWithRole: Role.Owner)).Questions;
        }

        public Task<PaginatedQuestionsResponse> GetPaginatedQuestionsAsync(
            int? page = null,
            int? pageSize = null,
            Role? getWithRole = null)
        {
            return _requestSender.GetFromJsonAsync<PaginatedQuestionsResponse>(
                requestUri: QuestionEndpointRoutes.GetPaginatedQuestionsRoute(
                    page, pageSize),
                getWithRole: getWithRole);
        }

        public Task<HttpResponseMessage> SendGetPaginatedQuestionsRequestAsync(
            int? page = null,
            int? pageSize = null,
            Role? sentFromRole = null)
        {
            return _requestSender.GetAsync(
                requestUri: QuestionEndpointRoutes.GetPaginatedQuestionsRoute(
                    page, pageSize),
                getWithRole: sentFromRole);
        }

        public Task<QuestionResponse> GetQuestionByIdAsAuthorizedAsync(
            Guid questionId)
        {
            return GetQuestionByIdAsync(
                questionId,
                getWithRole: Role.Owner);
        }

        public Task<QuestionResponse> GetQuestionByIdAsync(
            Guid questionId,
            Role? getWithRole = null)
        {
            return _requestSender.GetFromJsonAsync<QuestionResponse>(
                requestUri: QuestionEndpointRoutes.GetQuestionByIdRoute(questionId),
                getWithRole: getWithRole);
        }

        public Task<HttpResponseMessage> SendGetQuestionByIdRequestAsync(
            Guid questionId,
            Role? sentFromRole = null)
        {
            return _requestSender.GetAsync(
                requestUri: QuestionEndpointRoutes.GetQuestionByIdRoute(questionId),
                getWithRole: sentFromRole);
        }

        public Task<IEnumerable<TopicResponse>> GetQuestionTopicsAsAuthorizedUserAsync(
            Guid questionId)
        {
            return GetQuestionTopicsAsync(
                questionId,
                getWithRole: Role.Owner);
        }

        public Task<IEnumerable<TopicResponse>> GetQuestionTopicsAsync(
            Guid questionId,
            Role? getWithRole = null)
        {
            return _requestSender.GetFromJsonAsync<IEnumerable<TopicResponse>>(
                requestUri: QuestionEndpointRoutes.GetQuestionTopicsRoute(questionId),
                getWithRole: getWithRole);
        }

        public Task<HttpResponseMessage> SendGetQuestionTopicsRequestAsync(
            Guid questionId,
            Role? sentFromRole = null)
        {
            return _requestSender.GetAsync(
                requestUri: QuestionEndpointRoutes.GetQuestionTopicsRoute(questionId),
                getWithRole: sentFromRole);
        }

        public Task<IEnumerable<TicketResponse>> GetQuestionTicketsAsAuthorizedUserAsync(
            Guid questionId)
        {
            return GetQuestionTicketsAsync(
                questionId,
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

        public Task<HttpResponseMessage> SendGetQuestionTicketsRequestAsync(
            Guid questionId,
            Role? sentFromRole = null)
        {
            return _requestSender.GetAsync(
                requestUri: QuestionEndpointRoutes.GetQuestionTicketsRoute(questionId),
                getWithRole: sentFromRole);
        }

        public Task<HttpResponseMessage> SendGetRandomQuestionsRequestAsync(
            int? amount = null,
            Role? sentFromRole = null)
        {
            return _requestSender.GetAsync(
                requestUri: QuestionEndpointRoutes.GetRandomQuestionsRoute(amount),
                getWithRole: sentFromRole);
        }

        public Task<IEnumerable<QuestionResponse>> GetRandomQuestionsAsync(
            int? amount = null,
            Role? getWithRole = null)
        {
            return _requestSender.GetFromJsonAsync<IEnumerable<QuestionResponse>>(
                    requestUri: QuestionEndpointRoutes.GetRandomQuestionsRoute(amount),
                    getWithRole: getWithRole);
        }

        public Task<HttpResponseMessage> SendGetQuestionsForTheoryTestRequestAsync(
            Role? sentFromRole = null)
        {
            return _requestSender.GetAsync(
                requestUri: QuestionEndpointRoutes.GetQuestionsForTheoryTestRoute,
                getWithRole: sentFromRole);
        }

        public Task<IEnumerable<QuestionResponse>> GetQuestionsForTheoryTestAsync(
            Role? getWithRole = null)
        {
            return _requestSender.GetFromJsonAsync<IEnumerable<QuestionResponse>>(
                requestUri: QuestionEndpointRoutes.GetQuestionsForTheoryTestRoute,
                getWithRole: getWithRole);
        }
    }
}
