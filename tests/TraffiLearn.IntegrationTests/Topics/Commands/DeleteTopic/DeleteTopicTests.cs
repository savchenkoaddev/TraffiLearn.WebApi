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
            await TopicRequestSender.CreateValidTopicAsync();

            var allTopics = await TopicRequestSender.GetAllTopicsSortedByNumberAsync();

            var firstTopicId = allTopics.First().Id;

            var response = await RequestSender.DeleteAsync(
                requestUri: TopicEndpointRoutes.DeleteTopicRoute(firstTopicId));

            allTopics = await TopicRequestSender.GetAllTopicsSortedByNumberAsync();

            allTopics.Should().HaveCount(1);
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task DeleteTopic_IfUserIsNotEligible_ShouldReturn401StatusCode(
            Role role)
        {
            var topicId = Guid.NewGuid();

            var response = await RequestSender.DeleteWithRoleAsync(
                role,
                requestUri: TopicEndpointRoutes.DeleteTopicRoute(topicId));

            response.AssertForbiddenStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task DeleteTopic_IfUserIsNotEligible_TopicShouldNotBeDeleted(
            Role role)
        {
            await TopicRequestSender.CreateValidTopicAsync();

            var allTopics = await TopicRequestSender.GetAllTopicsSortedByNumberAsync();

            var firstTopicId = allTopics.First().Id;

            var response = await RequestSender.DeleteWithRoleAsync(
                role,
                requestUri: TopicEndpointRoutes.DeleteTopicRoute(firstTopicId));

            allTopics = await TopicRequestSender.GetAllTopicsSortedByNumberAsync();

            allTopics.Should().HaveCount(1);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task DeleteTopic_IfUserIsEligibleTopicIsNotFound_ShouldReturn404StatusCode(
            Role role)
        {
            var topicId = Guid.NewGuid();

            var response = await RequestSender.DeleteWithRoleAsync(
                role,
                requestUri: TopicEndpointRoutes.DeleteTopicRoute(topicId));

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task DeleteTopic_IfUserIsEligibleAndTopicIsFound_ShouldReturn204StatusCode(
            Role role)
        {
            await TopicRequestSender.CreateValidTopicAsync();

            var allTopics = await TopicRequestSender.GetAllTopicsSortedByNumberAsync();

            var firstTopicId = allTopics.First().Id;

            var response = await RequestSender.DeleteWithRoleAsync(
                role,
                requestUri: TopicEndpointRoutes.DeleteTopicRoute(firstTopicId));

            response.AssertNoContentStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task DeleteTopic_IfUserIsEligibleAndTopicIsFound_TopicShouldBeDeleted(
            Role role)
        {
            await TopicRequestSender.CreateValidTopicAsync();

            var allTopics = await TopicRequestSender.GetAllTopicsSortedByNumberAsync();

            var firstTopicId = allTopics.First().Id;

            await RequestSender.DeleteWithRoleAsync(
                role,
                requestUri: TopicEndpointRoutes.DeleteTopicRoute(firstTopicId));

            allTopics = await TopicRequestSender.GetAllTopicsSortedByNumberAsync();

            allTopics.Should().BeEmpty();
        }
    }
}
