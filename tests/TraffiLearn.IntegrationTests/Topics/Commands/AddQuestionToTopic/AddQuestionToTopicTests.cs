using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;
using TraffiLearn.IntegrationTests.Questions;

namespace TraffiLearn.IntegrationTests.Topics.Commands.AddQuestionToTopic
{
    public sealed class AddQuestionToTopicTests : TopicIntegrationTest
    {
        private readonly AuthorizedQuestionRequestSender _questionRequestSender;

        public AddQuestionToTopicTests(
            WebApplicationFactory factory)
            : base(factory)
        {
            _questionRequestSender = new AuthorizedQuestionRequestSender(RequestSender);
        }

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
        public async Task AddQuestionToTopic_IfUserEligibleButQuestionNotFound_ShouldReturn404StatusCode(
            Role role)
        {
            await TopicRequestSender.CreateValidTopicAsync();

            var allTopics = await TopicRequestSender.GetAllTopicsSortedByNumberAsync();

            var firstTopicId = allTopics.First().Id;

            var response = await RequestSender.PutAsync(
                requestUri: TopicEndpointRoutes.AddQuestionToTopicRoute(
                    questionId: Guid.NewGuid(),
                    topicId: firstTopicId),
                putWithRole: role);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task AddQuestionToTopic_IfUserEligibleButTopicNotFound_ShouldReturn404StatusCode(
            Role role)
        {
            await TopicRequestSender.CreateValidTopicAsync();

            var allTopics = await TopicRequestSender.GetAllTopicsSortedByNumberAsync();

            var firstTopicId = allTopics.First().Id;

            await _questionRequestSender.CreateValidQuestionAsync(
                topicIds: [firstTopicId]);

            var allQuestions = await _questionRequestSender.GetAllQuestionAsync();

            var firstQuestionId = allQuestions.First().Id;

            var response = await RequestSender.PutAsync(
                requestUri: TopicEndpointRoutes.AddQuestionToTopicRoute(
                    questionId: firstQuestionId,
                    topicId: Guid.NewGuid()),
                putWithRole: role);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task AddQuestionToTopic_IfUserEligibleButQuestionAlreadyAdded_ShouldReturn400StatusCode(
            Role role)
        {
            await SeedTopicAsync();
            var firstTopicId = await GetFirstTopicId();

            await _questionRequestSender.CreateValidQuestionAsync(
                topicIds: [firstTopicId]);

            var allQuestions = await _questionRequestSender.GetAllQuestionAsync();

            var firstQuestionId = allQuestions.First().Id;

            var response = await RequestSender.PutAsync(
                requestUri: TopicEndpointRoutes.AddQuestionToTopicRoute(
                    questionId: firstQuestionId,
                    topicId: firstTopicId),
                putWithRole: role);

            var secondResponse = await RequestSender.PutAsync(
                requestUri: TopicEndpointRoutes.AddQuestionToTopicRoute(
                    questionId: firstQuestionId,
                    topicId: firstTopicId),
                putWithRole: role);

            response.AssertBadRequestStatusCode();
            secondResponse.AssertBadRequestStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task AddQuestionToTopic_IfValidCase_ShouldReturn204StatusCode(
            Role role)
        {
            //await SeedTopicAsync();
            //var firstTopicId = await GetFirstTopicId();

            ////invar questionId = await _questionRequestSender.CreateValidQuestionAsync(
            ////    topicIds: [firstTopicId]);

            //var response = await RequestSender.PutAsync(
            //    requestUri: TopicEndpointRoutes.AddQuestionToTopicRoute(
            //        questionId: questionId,
            //        topicId: firstTopicId),
            //    putWithRole: role);

            //var secondResponse = await RequestSender.PutAsync(
            //    requestUri: TopicEndpointRoutes.AddQuestionToTopicRoute(
            //        questionId: questionId,
            //        topicId: firstTopicId),
            //    putWithRole: role);

            //response.AssertBadRequestStatusCode();
            //secondResponse.AssertBadRequestStatusCode();
        }

        private async Task<Guid> GetFirstTopicId()
        {
            var allTopics = await TopicRequestSender.GetAllTopicsSortedByNumberAsync();

            return allTopics.First().Id;
        }

        private async Task SeedTopicAsync()
        {
            await TopicRequestSender.CreateValidTopicAsync();
        }
    }
}
