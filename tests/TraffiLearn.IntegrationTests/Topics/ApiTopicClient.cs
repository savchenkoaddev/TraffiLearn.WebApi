using System.Net.Http.Json;
using TraffiLearn.Application.UseCases.Questions.DTO;
using TraffiLearn.Application.UseCases.Topics.Commands.Create;
using TraffiLearn.Application.UseCases.Topics.Commands.Update;
using TraffiLearn.Application.UseCases.Topics.DTO;
using TraffiLearn.Domain.Users.Roles;
using TraffiLearn.IntegrationTests.Helpers;
using TraffiLearn.IntegrationTests.Topics.Commands.CreateTopic;
using TraffiLearn.IntegrationTests.Topics.Commands.UpdateTopic;

namespace TraffiLearn.IntegrationTests.Topics
{
    public sealed class ApiTopicClient
    {
        private readonly RequestSender _requestSender;
        private readonly CreateTopicCommandFactory _createTopicCommandFactory;
        private readonly UpdateTopicCommandFactory _updateTopicCommandFactory;

        public ApiTopicClient(
            RequestSender requestSender,
            CreateTopicCommandFactory createTopicCommandFactory,
            UpdateTopicCommandFactory updateTopicCommandFactory)
        {
            _requestSender = requestSender;
            _createTopicCommandFactory = createTopicCommandFactory;
            _updateTopicCommandFactory = updateTopicCommandFactory;
        }

        public Task<Guid> CreateTopicAsync(
            Role? createdWithRole = null)
        {
            var command = _createTopicCommandFactory.CreateValidCommand();

            return CreateTopicAndGetIdAsync(
                command,
                createdWithRole);
        }

        public Task<Guid> CreateTopicAsAuthorizedAsync(
            CreateTopicCommand command)
        {
            return CreateTopicAndGetIdAsync(
                command,
                createdWithRole: Role.Owner);
        }

        public Task<Guid> CreateValidTopicAsAuthorizedAsync()
        {
            return CreateTopicAsync(
                createdWithRole: Role.Owner);
        }

        private async Task<Guid> CreateTopicAndGetIdAsync(
           CreateTopicCommand command,
           Role? createdWithRole = null)
        {
            var response = await SendCreateTopicRequestAsync(
                command,
                sentFromRole: createdWithRole);

            return await response.Content.ReadFromJsonAsync<Guid>();
        }

        public Task<HttpResponseMessage> SendCreateTopicRequestAsync(
            Role? sentFromRole = null)
        {
            var command = _createTopicCommandFactory.CreateValidCommand();

            return SendCreateTopicRequestAsync(
                command,
                sentFromRole: sentFromRole);
        }

        private Task<HttpResponseMessage> SendCreateTopicRequestAsync(
            CreateTopicCommand command,
            Role? sentFromRole = null)
        {
            return _requestSender.SendMultipartFormDataWithJsonAndFileRequest(
                method: HttpMethod.Post,
                requestUri: TopicEndpointRoutes.CreateTopicRoute,
                command,
                sentFromRole: sentFromRole);
        }

        public Task<IEnumerable<TopicResponse>> GetAllTopicsSortedByNumberAsync(
            Role? getWithRole = null)
        {
            return _requestSender.GetFromJsonAsync<IEnumerable<TopicResponse>>(
                requestUri: TopicEndpointRoutes.GetAllSortedTopicsByNumberRoute,
                getWithRole);
        }

        public Task<IEnumerable<TopicResponse>> GetAllTopicsSortedByNumberAsAuthorizedAsync()
        {
            return GetAllTopicsSortedByNumberAsync(
                getWithRole: Role.Owner);
        }

        public Task<HttpResponseMessage> SendGetAllTopicsSortedByNumberRequestAsync(
            Role? sentFromRole = null)
        {
            return _requestSender.GetAsync(
                requestUri: TopicEndpointRoutes.GetAllSortedTopicsByNumberRoute,
                getWithRole: sentFromRole);
        }

        public Task<HttpResponseMessage> SendGetTopicByIdRequestAsync(
            Guid topicId,
            Role? sentFromRole = null)
        {
            return _requestSender.GetAsync(
                requestUri: TopicEndpointRoutes.GetTopicByIdRoute(topicId),
                getWithRole: sentFromRole);
        }

        public Task<TopicResponse> GetTopicByIdAsync(
            Guid topicId,
            Role? sentFromRole = null)
        {
            return _requestSender.GetFromJsonAsync<TopicResponse>(
                requestUri: TopicEndpointRoutes.GetTopicByIdRoute(topicId),
                getWithRole: sentFromRole);
        }

        public Task<HttpResponseMessage> SendGetRandomTopicWithQuestionsAsync(
            Role? sentFromRole = null)
        {
            return _requestSender.GetAsync(
                requestUri: TopicEndpointRoutes.GetRandomTopicWithQuestionsRoute,
                getWithRole: sentFromRole);
        }

        public Task<TopicWithQuestionsResponse> GetRandomTopicWithQuestionsAsync(
            Role? sentFromRole = null)
        {
            return _requestSender.GetFromJsonAsync<TopicWithQuestionsResponse>(
                requestUri: TopicEndpointRoutes.GetRandomTopicWithQuestionsRoute,
                getWithRole: sentFromRole);
        }

        public Task<HttpResponseMessage> SendGetTopicQuestionsAsync(
            Guid topicId,
            Role? sentFromRole = null)
        {
            return _requestSender.GetAsync(
                requestUri: TopicEndpointRoutes.GetTopicQuestionsRoute(topicId),
                getWithRole: sentFromRole);
        }

        public Task<IEnumerable<QuestionResponse>> GetTopicQuestionsAsync(
            Guid topicId,
            Role? getWithRole = null)
        {
            return _requestSender.GetFromJsonAsync<IEnumerable<QuestionResponse>>(
                requestUri: TopicEndpointRoutes.GetTopicQuestionsRoute(topicId),
                getWithRole: getWithRole);
        }

        public Task<IEnumerable<QuestionResponse>> GetTopicQuestionsAsAuthorizedAsync(
           Guid topicId)
        {
            return GetTopicQuestionsAsync(
                topicId,
                getWithRole: Role.Owner);
        }

        public Task<HttpResponseMessage> SendDeleteTopicRequestAsync(
            Guid topicId,
            Role? sentFromRole = null)
        {
            return _requestSender.DeleteAsync(
                requestUri: TopicEndpointRoutes.DeleteTopicRoute(topicId),
                sentFromRole);
        }

        public Task<HttpResponseMessage> SendRemoveQuestionFromTopicRequestAsync(
            Guid questionId,
            Guid topicId,
            Role? sentFromRole = null)
        {
            return _requestSender.PutAsync(
                requestUri: TopicEndpointRoutes.RemoveQuestionFromTopicRoute(
                    questionId,
                    topicId),
                putWithRole: sentFromRole);
        }

        public Task<HttpResponseMessage> SendAddQuestionToTopicRequestAsync(
           Guid questionId,
           Guid topicId,
           Role? sentFromRole = null)
        {
            return _requestSender.PutAsync(
                requestUri: TopicEndpointRoutes.AddQuestionToTopicRoute(
                    questionId,
                    topicId),
                putWithRole: sentFromRole);
        }

        public Task<HttpResponseMessage> SendValidUpdateTopicRequestAsync(
            Guid topicId,
            Role? updatedWithRole = null)
        {
            var command = _updateTopicCommandFactory.CreateValidCommand(topicId);

            return SendUpdateTopicRequestAsync(
                command,
                updatedWithRole: updatedWithRole);
        }

        public Task<HttpResponseMessage> SendUpdateTopicRequestAsync(
            UpdateTopicCommand command,
            Role? updatedWithRole = null)
        {
            return _requestSender.SendMultipartFormDataWithJsonAndFileRequest(
                method: HttpMethod.Put,
                requestUri: TopicEndpointRoutes.UpdateTopicRoute,
                value: command,
                sentFromRole: updatedWithRole);
        }
    }
}
