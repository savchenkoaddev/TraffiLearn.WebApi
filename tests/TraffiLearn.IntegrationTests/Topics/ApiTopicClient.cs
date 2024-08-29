using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Application.Topics.Commands.Create;
using TraffiLearn.Application.Topics.Commands.Update;
using TraffiLearn.Application.Topics.DTO;
using TraffiLearn.Domain.Aggregates.Users.Enums;
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

        public Task<HttpResponseMessage> SendCreateTopicRequestAsync(
            Role? sentFromRole = null)
        {
            var command = _createTopicCommandFactory.CreateValidCommand();

            return _requestSender.SendJsonAsync(
                method: HttpMethod.Post,
                requestUri: TopicEndpointRoutes.CreateTopicRoute,
                value: command,
                sentFromRole: sentFromRole);
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

        private Task<Guid> CreateTopicAndGetIdAsync(
           CreateTopicCommand command,
           Role? createdWithRole = null)
        {
            return _requestSender
                .SendAndGetJsonAsync<CreateTopicCommand, Guid>(
                    method: HttpMethod.Post,
                    requestUri: TopicEndpointRoutes.CreateTopicRoute,
                    value: command,
                    sentWithRole: createdWithRole);
        }

        private Task<HttpResponseMessage> SendCreateTopicRequestAsync(
            CreateTopicCommand command,
            Role? sentFromRole = null)
        {
            return _requestSender.SendJsonAsync(
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
            Role? sentWithRole = null)
        {
            return _requestSender.GetAsync(
                requestUri: TopicEndpointRoutes.GetAllSortedTopicsByNumberRoute,
                getWithRole: sentWithRole);
        }

        public Task<HttpResponseMessage> SendGetTopicByIdRequestAsync(
            Guid topicId,
            Role? sentWithRole = null)
        {
            return _requestSender.GetAsync(
                requestUri: TopicEndpointRoutes.GetTopicByIdRoute(topicId),
                getWithRole: sentWithRole);
        }

        public Task<TopicResponse> GetTopicByIdAsync(
            Guid topicId,
            Role? sentWithRole = null)
        {
            return _requestSender.GetFromJsonAsync<TopicResponse>(
                requestUri: TopicEndpointRoutes.GetTopicByIdRoute(topicId),
                getWithRole: sentWithRole);
        }

        public Task<HttpResponseMessage> SendGetRandomTopicWithQuestionsAsync(
            Role? sentWithRole = null)
        {
            return _requestSender.GetAsync(
                requestUri: TopicEndpointRoutes.GetRandomTopicWithQuestionsRoute,
                getWithRole: sentWithRole);
        }

        public Task<TopicWithQuestionsResponse> GetRandomTopicWithQuestionsAsync(
            Role? sentWithRole = null)
        {
            return _requestSender.GetFromJsonAsync<TopicWithQuestionsResponse>(
                requestUri: TopicEndpointRoutes.GetRandomTopicWithQuestionsRoute,
                getWithRole: sentWithRole);
        }

        public Task<HttpResponseMessage> SendGetTopicQuestionsAsync(
            Guid topicId,
            Role? sentWithRole = null)
        {
            return _requestSender.GetAsync(
                requestUri: TopicEndpointRoutes.GetTopicQuestionsRoute(topicId),
                getWithRole: sentWithRole);
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
            return _requestSender.PutAsJsonAsync(
                requestUri: TopicEndpointRoutes.UpdateTopicRoute,
                request: command,
                putWithRole: updatedWithRole);
        }
    }
}
