using TraffiLearn.Domain.Aggregates.Users.Roles;
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
            var response = await ApiTopicClient.SendGetTopicByIdRequestAsync(
                topicId: Guid.NewGuid(),
                sentFromRole: null);

            response.AssertUnauthorizedStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetTopicById_IfTopicIsNotFound_ShouldReturn404StatusCode(
            Role eligibleRole)
        {
            var response = await ApiTopicClient.SendGetTopicByIdRequestAsync(
                topicId: Guid.NewGuid(),
                sentFromRole: eligibleRole);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetTopicById_IfValidCase_ShouldReturn200StatusCode(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var response = await ApiTopicClient.SendGetTopicByIdRequestAsync(
                topicId: topicId,
                sentFromRole: eligibleRole);

            response.AssertOkStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetTopicById_IfValidCase_ShouldReturnValidTopic(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var response = await ApiTopicClient.GetTopicByIdAsync(
                topicId: topicId,
                sentFromRole: eligibleRole);

            response.Should().NotBeNull();
            response.Id.Should().Be(topicId);
        }
    }
}
