using System.Net.Http.Json;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Application.Topics.Commands.Create;
using TraffiLearn.Application.Topics.DTO;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Helpers;
using TraffiLearn.IntegrationTests.Topics.Commands.CreateTopic;

namespace TraffiLearn.IntegrationTests.Topics
{
    public sealed class ApiTopicClient
    {
        private readonly RequestSender _requestSender;

        public ApiTopicClient(RequestSender requestSender)
        {
            _requestSender = requestSender;
        }

        public async Task<Guid> CreateValidTopicAsync()
        {
            var command = CreateTopicCommandFactory.CreateValidCommand();

            var response = await _requestSender.SendJsonAsync(
                method: HttpMethod.Post,
                requestUri: TopicEndpointRoutes.CreateTopicRoute,
                command,
                sentFromRole: Role.Owner);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Guid>();
        }

        public async Task<Guid> CreateTopicAsync(
            CreateTopicCommand command)
        {
            var response = await _requestSender.SendJsonAsync(
                method: HttpMethod.Post,
                requestUri: TopicEndpointRoutes.CreateTopicRoute,
                command,
                sentFromRole: Role.Owner);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Guid>();
        }

        public Task<IEnumerable<TopicResponse>> GetAllTopicsSortedByNumberAsync()
        {
            return _requestSender.GetFromJsonAsync<IEnumerable<TopicResponse>>(
                requestUri: TopicEndpointRoutes.GetAllSortedTopicsByNumberRoute,
                getWithRole: Role.Owner);
        }

        public Task<IEnumerable<QuestionResponse>> GetTopicQuestionsAsync(
            Guid topicId)
        {
            return _requestSender.GetFromJsonAsync<IEnumerable<QuestionResponse>>(
                requestUri: TopicEndpointRoutes.GetTopicQuestionsRoute(topicId),
                getWithRole: Role.Owner);
        }
    }
}
