using FluentAssertions;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;
using TraffiLearn.IntegrationTests.Questions;

namespace TraffiLearn.IntegrationTests.Topics.Commands.AddQuestionToTopic
{
    public sealed class AddQuestionToTopicTests : TopicIntegrationTest
    {
        public AddQuestionToTopicTests(
            WebApplicationFactory factory)
            : base(factory)
        { }

        [Fact]
        public async Task AddQuestionToTopic_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
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
            var response = await RequestSender.PutAsync(
                requestUri: TopicEndpointRoutes.AddQuestionToTopicRoute(
                    questionId: Guid.NewGuid(),
                    topicId: Guid.NewGuid()),
                putWithRole: role);

            response.AssertForbiddenStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task AddQuestionToTopic_IfUserIsEligibleButQuestionNotFound_ShouldReturn404StatusCode(
            Role role)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsync();

            var response = await RequestSender.PutAsync(
                requestUri: TopicEndpointRoutes.AddQuestionToTopicRoute(
                    questionId: Guid.NewGuid(),
                    topicId: topicId),
                putWithRole: role);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task AddQuestionToTopic_IfUserIsEligibleButTopicNotFound_ShouldReturn404StatusCode(
            Role role)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsync();

            var questionId = await ApiQuestionClient.CreateValidQuestionAsync(
                topicIds: [topicId]);

            var response = await RequestSender.PutAsync(
                requestUri: TopicEndpointRoutes.AddQuestionToTopicRoute(
                    questionId: questionId,
                    topicId: Guid.NewGuid()),
                putWithRole: role);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task AddQuestionToTopic_IfUserIsEligibleButQuestionAlreadyAdded_ShouldReturn400StatusCode(
            Role role)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsync();

            var questionId = await ApiQuestionClient.CreateValidQuestionAsync(
                topicIds: [topicId]);

            var response = await RequestSender.PutAsync(
                requestUri: TopicEndpointRoutes.AddQuestionToTopicRoute(
                    questionId: questionId,
                    topicId: topicId),
                putWithRole: role);

            response.AssertBadRequestStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task AddQuestionToTopic_IfValidCase_ShouldReturn204StatusCode(
            Role role)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsync();

            var questionId = await ApiQuestionClient.CreateValidQuestionAsync(
                topicIds: [topicId]);

            var newTopicId = await ApiTopicClient.CreateValidTopicAsync();

            var response = await RequestSender.PutAsync(
                requestUri: TopicEndpointRoutes.AddQuestionToTopicRoute(
                    questionId: questionId,
                    topicId: newTopicId),
                putWithRole: role);

            response.AssertNoContentStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task AddQuestionToTopic_IfValidCase_QuestionShouldBeAddedToTopic(
            Role role)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsync();

            var questionId = await ApiQuestionClient.CreateValidQuestionAsync(
                topicIds: [topicId]);

            var newTopicId = await ApiTopicClient.CreateValidTopicAsync();

            var response = await RequestSender.PutAsync(
                requestUri: TopicEndpointRoutes.AddQuestionToTopicRoute(
                    questionId: questionId,
                    topicId: newTopicId),
                putWithRole: role);

            var topicQuestions = await ApiTopicClient.GetTopicQuestionsAsync(newTopicId);

            topicQuestions.Should().HaveCount(1);
            topicQuestions.First().Id.Should().Be(questionId);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task AddQuestionToTopic_IfValidCase_TopicShouldBeAddedToQuestion(
            Role role)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsync();

            var questionId = await ApiQuestionClient.CreateValidQuestionAsync(
                topicIds: [topicId]);

            var newTopicId = await ApiTopicClient.CreateValidTopicAsync();

            var response = await RequestSender.PutAsync(
                requestUri: TopicEndpointRoutes.AddQuestionToTopicRoute(
                    questionId: questionId,
                    topicId: newTopicId),
                putWithRole: role);

            var questionTopics = await ApiQuestionClient.GetQuestionTopicsAsync(questionId);

            questionTopics.Should().HaveCount(2);
            questionTopics.Any(t => t.Id == newTopicId).Should().BeTrue();
        }
    }
}
