using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;

namespace TraffiLearn.IntegrationTests.Questions.Queries.GetAllQuestions
{
    public sealed class GetAllQuestionsTests : QuestionIntegrationTest
    {
        public GetAllQuestionsTests(
            WebApplicationFactory factory)
            : base(factory)
        { }

        [Fact]
        public async Task GetAllQuestions_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await ApiQuestionClient
                .SendGetAllQuestionsRequestAsync(
                    sentFromRole: null);

            response.AssertUnauthorizedStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetAllQuestions_IfNoQuestionsExist_ShouldReturn200StatusCode(
            Role eligibleRole)
        {
            var response = await ApiQuestionClient
                .SendGetAllQuestionsRequestAsync(
                    sentFromRole: eligibleRole);

            response.AssertOkStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetAllQuestions_IfNoQuestionsExist_ShouldReturnEmptyCollection(
            Role eligibleRole)
        {
            var allQuestions = await ApiQuestionClient
                .GetAllQuestionsAsync(
                    getWithRole: eligibleRole);

            allQuestions.Should().NotBeNull();
            allQuestions.Should().BeEmpty();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetAllQuestions_IfValidCase_ShouldReturn200StatusCode(
            Role eligibleRole)
        {
            await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var response = await ApiQuestionClient
                .SendGetAllQuestionsRequestAsync(
                    sentFromRole: eligibleRole);

            response.AssertOkStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetAllQuestions_IfValidCase_ShouldReturnAllQuestions(
            Role eligibleRole)
        {
            List<Guid> questionIds = [
                await ApiQuestionClient
                    .CreateValidQuestionWithTopicAsAuthorizedAsync(),

                await ApiQuestionClient
                    .CreateValidQuestionWithTopicAsAuthorizedAsync()
            ];

            var allQuestions = await ApiQuestionClient
                .GetAllQuestionsAsync(
                    getWithRole: eligibleRole);

            var allQuestionIds = allQuestions.Select(q => q.Id);

            allQuestionIds.Should().BeEquivalentTo(questionIds);
        }
    }
}
