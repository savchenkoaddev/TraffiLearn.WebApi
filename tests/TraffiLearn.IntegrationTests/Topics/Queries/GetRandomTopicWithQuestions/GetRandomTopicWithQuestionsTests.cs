using FluentAssertions;
using TraffiLearn.Application.Topics.DTO;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;
using TraffiLearn.IntegrationTests.Questions;

namespace TraffiLearn.IntegrationTests.Topics.Queries.GetRandomTopicWithQuestions
{
    public sealed class GetRandomTopicWithQuestionsTests : TopicIntegrationTest
    {
        private readonly ApiQuestionClient _apiQuestionClient;

        public GetRandomTopicWithQuestionsTests(
            WebApplicationFactory factory)
            : base(factory)
        {
            _apiQuestionClient = new ApiQuestionClient(RequestSender);
        }

        [Fact]
        public async Task GetRandomTopicWithQuestions_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await RequestSender.GetAsync(
                requestUri: TopicEndpointRoutes.GetRandomTopicWithQuestionsRoute);

            response.AssertUnauthorizedStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetRandomTopicWithQuestions_IfUserIsEligibleButNoTopicsExist_ShouldReturn500StatusCode(
            Role role)
        {
            var response = await RequestSender.GetAsync(
                requestUri: TopicEndpointRoutes.GetRandomTopicWithQuestionsRoute,
                getFromRole: role);

            response.AssertInternalServerErrorStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetRandomTopicWithQuestions_IfUserIsEligibleAndTopicDoesNotContainQuestions_ShouldReturn200StatusCode(
            Role role)
        {
            await ApiTopicClient.CreateValidTopicAsync();

            var response = await RequestSender.GetAsync(
                requestUri: TopicEndpointRoutes.GetRandomTopicWithQuestionsRoute,
                getFromRole: role);

            response.AssertOkStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetRandomTopicWithQuestions_IfUserIsEligibleAndTopicDoesNotContainQuestions_ShouldReturnTopicWithEmptyQuestions(
            Role role)
        {
            await ApiTopicClient.CreateValidTopicAsync();

            var topicWithQuestions = await RequestSender.GetFromJsonAsync<TopicWithQuestionsResponse>(
                requestUri: TopicEndpointRoutes.GetRandomTopicWithQuestionsRoute,
                getFromRole: role);

            topicWithQuestions.Questions.Should().NotBeNull();
            topicWithQuestions.Questions.Should().BeEmpty();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetRandomTopicWithQuestions_IfUserIsEligibleAndTopicDoesNotContainQuestions_TopicShouldBeCorrect(
            Role role)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsync();

            var topicWithQuestions = await RequestSender.GetFromJsonAsync<TopicWithQuestionsResponse>(
                requestUri: TopicEndpointRoutes.GetRandomTopicWithQuestionsRoute,
                getFromRole: role);

            topicWithQuestions.TopicId.Should().Be(topicId);
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetRandomTopicWithQuestions_IfValidCase_ShouldReturn200StatusCode(
            Role role)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsync();

            var questionId = await _apiQuestionClient.CreateValidQuestionAsync(
                topicIds: [topicId]);

            var response = await RequestSender.GetAsync(
                requestUri: TopicEndpointRoutes.GetRandomTopicWithQuestionsRoute,
                getFromRole: role);

            response.AssertOkStatusCode();
        }

        /* The endpoint is not being tested completelly,
          due to the inability to check for randomness in the correct way */
        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetRandomTopicWithQuestions_IfOnlyOneTopicExists_ShouldReturnRandomTopicWithQuestions(
            Role role)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsync();

            var questionId = await _apiQuestionClient.CreateValidQuestionAsync(
                topicIds: [topicId]);

            var topicWithQuestions = await RequestSender.GetFromJsonAsync<TopicWithQuestionsResponse>(
                requestUri: TopicEndpointRoutes.GetRandomTopicWithQuestionsRoute,
                getFromRole: role);

            topicWithQuestions.Questions.Should().HaveCount(1);
            topicWithQuestions.Questions.First().Id.Should().Be(questionId);
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetRandomTopicWithQuestions_IfOnlyOneTopicExists_TopicShouldBeCorrect(
            Role role)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsync();

            var questionId = await _apiQuestionClient.CreateValidQuestionAsync(
                topicIds: [topicId]);

            var topicWithQuestions = await RequestSender.GetFromJsonAsync<TopicWithQuestionsResponse>(
                requestUri: TopicEndpointRoutes.GetRandomTopicWithQuestionsRoute,
                getFromRole: role);

            topicWithQuestions.TopicId.Should().Be(topicId);
        }
    }
}
