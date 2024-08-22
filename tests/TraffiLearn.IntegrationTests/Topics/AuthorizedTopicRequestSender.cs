using TraffiLearn.Application.Topics.DTO;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Helpers;
using TraffiLearn.IntegrationTests.Topics.Commands.CreateTopic;

namespace TraffiLearn.IntegrationTests.Topics
{
    public sealed class AuthorizedTopicRequestSender
    {
        private readonly RequestSender _requestSender;

        public AuthorizedTopicRequestSender(RequestSender requestSender)
        {
            _requestSender = requestSender;
        }

        public async Task CreateValidTopicAsync()
        {
            var command = CreateTopicFixtureFactory.CreateValidCommand();

            var response = await _requestSender.SendJsonRequestWithRole(
                role: Role.Owner,
                method: HttpMethod.Post,
                requestUri: TopicEndpointRoutes.CreateTopicRoute,
                command);

            response.EnsureSuccessStatusCode();
        }

        public Task<IEnumerable<TopicResponse>> GetAllTopicsSortedByNumberAsync()
        {
            return _requestSender.GetFromJsonWithRoleAsync<IEnumerable<TopicResponse>>(
                role: Role.Owner,
                requestUri: TopicEndpointRoutes.GetAllSortedTopicsByNumberRoute);
        }
    }
}
