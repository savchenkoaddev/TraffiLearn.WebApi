using FluentAssertions;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;
using TraffiLearn.IntegrationTests.Questions;

namespace TraffiLearn.IntegrationTests.Topics.Queries.GetTopicQuestions
{
    public sealed class GetTopicQuestionsTests : TopicIntegrationTest
    {
        private readonly ApiQuestionClient _apiQuestionClient;

        public GetTopicQuestionsTests(
            WebApplicationFactory factory)
            : base(factory)
        {
            _apiQuestionClient = new ApiQuestionClient(RequestSender);
        }

        [Fact]
        public async Task GetTopicQuestions_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await RequestSender.GetAsync(
                requestUri: TopicEndpointRoutes.GetTopicQuestionsRoute(
                    topicId: Guid.NewGuid()));

            response.AssertUnauthorizedStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetTopicQuestions_IfUserIsEligibleButTopicNotFound_ShouldReturn404StatusCode(
            Role role)
        {
            var response = await RequestSender.GetAsync(
                requestUri: TopicEndpointRoutes.GetTopicQuestionsRoute(
                    topicId: Guid.NewGuid()),
                getFromRole: role);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetTopicQuestions_IfUserIsEligibleButTopicContainsNoQuestions_ShouldReturn200StatusCode(
            Role role)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsync();

            var response = await RequestSender.GetAsync(
                requestUri: TopicEndpointRoutes.GetTopicQuestionsRoute(
                    topicId: topicId),
                getFromRole: role);

            response.AssertOkStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetTopicQuestions_IfUserIsEligibleButTopicContainsNoQuestions_ShouldReturnEmptyCollection(
            Role role)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsync();

            var topicQuestions = await RequestSender.GetFromJsonAsync<IEnumerable<QuestionResponse>>(
                requestUri: TopicEndpointRoutes.GetTopicQuestionsRoute(
                    topicId: topicId),
                getFromRole: role);

            topicQuestions.Should().NotBeNull();
            topicQuestions.Should().BeEmpty();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetTopicQuestions_IfValidCase_ShouldReturn200StatusCode(
           Role role)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsync();

            var questionId = await _apiQuestionClient.CreateValidQuestionAsync(
                topicIds: [topicId]);

            var response = await RequestSender.GetAsync(
                requestUri: TopicEndpointRoutes.GetTopicQuestionsRoute(
                    topicId: topicId),
                getFromRole: role);

            response.AssertOkStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetTopicQuestions_IfValidCase_ShouldReturnTopicQuestions(
            Role role)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsync();

            var questionId = await _apiQuestionClient.CreateValidQuestionAsync(
                topicIds: [topicId]);

            var topicQuestions = await RequestSender.GetFromJsonAsync<IEnumerable<QuestionResponse>>(
                requestUri: TopicEndpointRoutes.GetTopicQuestionsRoute(
                    topicId: topicId),
                getFromRole: role);

            topicQuestions.Should().HaveCount(1);
            topicQuestions.First().Id.Should().Be(questionId);
        }
    }
}
