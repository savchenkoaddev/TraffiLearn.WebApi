using FluentAssertions;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;
using TraffiLearn.IntegrationTests.Questions;

namespace TraffiLearn.IntegrationTests.Topics.Commands.RemoveQuestionFromTopic
{
    public sealed class RemoveQuestionFromTopicTests : TopicIntegrationTest
    {
        private readonly ApiQuestionClient _apiQuestionClient;

        public RemoveQuestionFromTopicTests(
            WebApplicationFactory factory)
            : base(factory)
        {
            _apiQuestionClient = new ApiQuestionClient(RequestSender);
        }

        [Fact]
        public async Task RemoveQuestionFromTopic_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await RequestSender.PutAsync(
                requestUri: TopicEndpointRoutes.RemoveQuestionFromTopicRoute(
                    questionId: Guid.NewGuid(),
                    topicId: Guid.NewGuid()));

            response.AssertUnauthorizedStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task RemoveQuestionFromTopic_IfUserIsNotEligible_ShouldReturn403StatusCode(
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
        public async Task RemoveQuestionFromTopic_IfUserIsEligibleButQuestionNotFound_ShouldReturn404StatusCode(
            Role role)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsync();

            var response = await RequestSender.PutAsync(
                requestUri: TopicEndpointRoutes.RemoveQuestionFromTopicRoute(
                    questionId: Guid.NewGuid(),
                    topicId: topicId),
                putWithRole: role);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task RemoveQuestionFromTopic_IfUserIsEligibleButTopicNotFound_ShouldReturn404StatusCode(
            Role role)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsync();

            var questionId = await _apiQuestionClient.CreateValidQuestionAsync(
                topicIds: [topicId]);

            var response = await RequestSender.PutAsync(
                requestUri: TopicEndpointRoutes.RemoveQuestionFromTopicRoute(
                    questionId: questionId,
                    topicId: Guid.NewGuid()),
                putWithRole: role);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task RemoveQuestionFromTopic_IfUserIsEligibleButQuestionIsNotAdded_ShouldReturn404StatusCode(
            Role role)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsync();

            var questionId = await _apiQuestionClient.CreateValidQuestionAsync(
                topicIds: [topicId]);

            var newTopicId = await ApiTopicClient.CreateValidTopicAsync();

            var newQuestionId = await _apiQuestionClient.CreateValidQuestionAsync(
                topicIds: [newTopicId]);

            var response = await RequestSender.PutAsync(
                requestUri: TopicEndpointRoutes.RemoveQuestionFromTopicRoute(
                    questionId: questionId,
                    topicId: newTopicId),
                putWithRole: role);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task RemoveQuestionFromTopic_IfValidCase_ShouldReturn204StatusCode(
            Role role)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsync();

            var questionId = await _apiQuestionClient.CreateValidQuestionAsync(
                topicIds: [topicId]);

            var response = await RequestSender.PutAsync(
                requestUri: TopicEndpointRoutes.RemoveQuestionFromTopicRoute(
                    questionId: questionId,
                    topicId: topicId),
                putWithRole: role);

            response.AssertNoContentStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task RemoveQuestionFromTopic_IfValidCase_QuestionShouldBeRemovedFromTopic(
            Role role)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsync();

            var questionId = await _apiQuestionClient.CreateValidQuestionAsync(
                topicIds: [topicId]);

            var response = await RequestSender.PutAsync(
                requestUri: TopicEndpointRoutes.RemoveQuestionFromTopicRoute(
                    questionId: questionId,
                    topicId: topicId),
                putWithRole: role);

            var topicQuestions = await ApiTopicClient.GetTopicQuestionsAsync(topicId);

            topicQuestions.Should().BeEmpty();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task RemoveQuestionFromTopic_IfValidCase_TopicShouldBeRemovedFromQuestion(
            Role role)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsync();

            var questionId = await _apiQuestionClient.CreateValidQuestionAsync(
                topicIds: [topicId]);

            var response = await RequestSender.PutAsync(
                requestUri: TopicEndpointRoutes.RemoveQuestionFromTopicRoute(
                    questionId: questionId,
                    topicId: topicId),
                putWithRole: role);

            var questionTopics = await _apiQuestionClient.GetQuestionTopicsAsync(questionId);

            questionTopics.Should().BeEmpty();
        }
    }
}
