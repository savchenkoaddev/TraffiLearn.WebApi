using FluentAssertions;
using TraffiLearn.Application.Topics.DTO;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;

namespace TraffiLearn.IntegrationTests.Topics.Queries.GetTopicById
{
    public sealed class GetTopicByIdTests : TopicIntegrationTest
    {
        public GetTopicByIdTests(
            WebApplicationFactory factory)
            : base(factory)
        { }

        [Fact]
        public async Task GetTopicById_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await RequestSender.GetAsync(
                requestUri: TopicEndpointRoutes.GetTopicByIdRoute(
                    topicId: Guid.NewGuid()));

            response.AssertUnauthorizedStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetTopicById_IfUserIsEligibleButTopicNotFound_ShouldReturn404StatusCode(
            Role role)
        {
            var response = await RequestSender.GetAsync(
                requestUri: TopicEndpointRoutes.GetTopicByIdRoute(
                    topicId: Guid.NewGuid()),
                getWithRole: role);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetTopicById_IfValidCase_ShouldReturn200StatusCode(
            Role role)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsync();

            var response = await RequestSender.GetAsync(
                requestUri: TopicEndpointRoutes.GetTopicByIdRoute(
                    topicId: topicId),
                getWithRole: role);

            response.AssertOkStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetTopicById_IfValidCase_ShouldReturnValidTopic(
            Role role)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsync();

            var response = await RequestSender.GetFromJsonAsync<TopicResponse>(
                requestUri: TopicEndpointRoutes.GetTopicByIdRoute(
                    topicId: topicId),
                getWithRole: role);

            response.Should().NotBeNull();
            response.Id.Should().Be(topicId);
        }
    }
}
