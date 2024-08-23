using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;

namespace TraffiLearn.IntegrationTests.Topics.Commands.AddQuestionToTopic
{
    public sealed class AddQuestionToTopicTests : TopicIntegrationTest
    {
        public AddQuestionToTopicTests(
            WebApplicationFactory factory)
            : base(factory)
        { }

        [Fact]
        public async Task AddQuestionToTopic_IfUserNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await RequestSender.PutAsync(
                requestUri: TopicEndpointRoutes.AddQuestionToTopicRoute(
                    questionId: Guid.NewGuid(),
                    topicId: Guid.NewGuid()));

            response.AssertUnauthorizedStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task AddQuestionToTopic_IfUserIsNotEligible_ShouldReturn403StatusCode(
            Role role)
        {
            var response = await RequestSender.PutWithRoleAsync(
                role,
                requestUri: TopicEndpointRoutes.AddQuestionToTopicRoute(
                    questionId: Guid.NewGuid(),
                    topicId: Guid.NewGuid()));

            response.AssertForbiddenStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task AddQuestionToTopic_IfUserEligibleButQuestionNotFound_ShouldReturn404StatusCode(
            Role role)
        {
            await TopicRequestSender.CreateValidTopicAsync();

            var allTopics = await TopicRequestSender.GetAllTopicsSortedByNumberAsync();

            var firstTopicId = allTopics.First().Id;

            var response = await RequestSender.PutWithRoleAsync(
                role,
                requestUri: TopicEndpointRoutes.AddQuestionToTopicRoute(
                    questionId: Guid.NewGuid(),
                    topicId: firstTopicId));

            response.AssertNotFoundStatusCode();
        }
    }
}
