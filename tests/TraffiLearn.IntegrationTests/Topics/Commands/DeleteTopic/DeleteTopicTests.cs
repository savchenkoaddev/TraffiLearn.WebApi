using FluentAssertions;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;

namespace TraffiLearn.IntegrationTests.Topics.Commands.DeleteTopic
{
    public sealed class DeleteTopicTests : TopicIntegrationTest
    {
        public DeleteTopicTests(
            WebApplicationFactory factory)
            : base(factory)
        { }

        [Fact]
        public async Task DeleteTopic_IfUserNotAuthenticated_ShouldReturn401StatusCode()
        {
            var topicId = Guid.NewGuid();

            var response = await RequestSender.DeleteAsync(
                requestUri: TopicEndpointRoutes.DeleteTopicRoute(topicId));

            response.AssertUnauthorizedStatusCode();
        }

        [Fact]
        public async Task DeleteTopic_IfUserNotAuthenticated_TopicShouldNotBeDeleted()
        {
            var topicId = await TopicRequestSender.CreateValidTopicAsync();

            var response = await RequestSender.DeleteAsync(
                requestUri: TopicEndpointRoutes.DeleteTopicRoute(topicId));

            var allTopics = await TopicRequestSender.GetAllTopicsSortedByNumberAsync();

            allTopics.Should().HaveCount(1);
            allTopics.First().Id.Should().Be(topicId);
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task DeleteTopic_IfUserIsNotEligible_ShouldReturn401StatusCode(
            Role role)
        {
            var topicId = Guid.NewGuid();

            var response = await RequestSender.DeleteAsync(
                requestUri: TopicEndpointRoutes.DeleteTopicRoute(topicId),
                deletedWithRole: role);

            response.AssertForbiddenStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task DeleteTopic_IfUserIsNotEligible_TopicShouldNotBeDeleted(
            Role role)
        {
            var topicId = await TopicRequestSender.CreateValidTopicAsync();

            var response = await RequestSender.DeleteAsync(
                requestUri: TopicEndpointRoutes.DeleteTopicRoute(topicId),
                deletedWithRole: role);

            var allTopics = await TopicRequestSender.GetAllTopicsSortedByNumberAsync();

            allTopics.Should().HaveCount(1);
            allTopics.First().Id.Should().Be(topicId);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task DeleteTopic_IfUserIsEligibleTopicIsNotFound_ShouldReturn404StatusCode(
            Role role)
        {
            var topicId = Guid.NewGuid();

            var response = await RequestSender.DeleteAsync(
                requestUri: TopicEndpointRoutes.DeleteTopicRoute(topicId),
                deletedWithRole: role);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task DeleteTopic_IfUserIsEligibleAndTopicIsFound_ShouldReturn204StatusCode(
            Role role)
        {
            var topicId = await TopicRequestSender.CreateValidTopicAsync();

            var response = await RequestSender.DeleteAsync(
                requestUri: TopicEndpointRoutes.DeleteTopicRoute(topicId),
                deletedWithRole: role);

            response.AssertNoContentStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task DeleteTopic_IfUserIsEligibleAndTopicIsFound_TopicShouldBeDeleted(
            Role role)
        {
            var topicId = await TopicRequestSender.CreateValidTopicAsync();

            await RequestSender.DeleteAsync(
                requestUri: TopicEndpointRoutes.DeleteTopicRoute(topicId),
                deletedWithRole: role);

            var allTopics = await TopicRequestSender.GetAllTopicsSortedByNumberAsync();

            allTopics.Should().BeEmpty();
        }
    }
}
