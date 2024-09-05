using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;

namespace TraffiLearn.IntegrationTests.Questions.Queries.GetQuestionTopics
{
    public sealed class GetQuestionTopicsTests : QuestionIntegrationTest
    {
        public GetQuestionTopicsTests(
            WebApplicationFactory factory)
            : base(factory)
        { }

        [Fact]
        public async Task GetQuestionTopics_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await ApiQuestionClient.SendGetQuestionTopicsRequestAsync(
                questionId: Guid.NewGuid(),
                sentFromRole: null);

            response.AssertUnauthorizedStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetQuestionTopics_IfQuestionIsNotFound_ShouldReturn404StatusCode(
            Role eligibleRole)
        {
            var response = await ApiQuestionClient.SendGetQuestionTopicsRequestAsync(
                questionId: Guid.NewGuid(),
                sentFromRole: eligibleRole);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetQuestionTopics_IfValidCase_ShouldReturn200StatusCode(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var response = await ApiQuestionClient.SendGetQuestionTopicsRequestAsync(
                questionId: questionId,
                sentFromRole: eligibleRole);

            response.AssertOkStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetQuestionTopics_IfValidCase_ShouldReturnQuestionTopics(
            Role eligibleRole)
        {
            List<Guid> topicIds = [
                await ApiTopicClient.CreateValidTopicAsAuthorizedAsync(),
                await ApiTopicClient.CreateValidTopicAsAuthorizedAsync(),
            ];

            await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var questionId = await ApiQuestionClient
                .CreateValidQuestionAsAuthorizedAsync(
                    topicIds: topicIds);

            var questionTopics = await ApiQuestionClient.GetQuestionTopicsAsync(
                questionId: questionId,
                getWithRole: eligibleRole);

            var questionTopicIds = questionTopics.Select(x => x.Id);

            questionTopicIds.Should().HaveCount(topicIds.Count);
            questionTopicIds.Should().BeEquivalentTo(topicIds);
        }
    }
}
