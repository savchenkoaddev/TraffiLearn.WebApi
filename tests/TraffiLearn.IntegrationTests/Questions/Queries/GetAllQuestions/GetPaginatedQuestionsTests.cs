using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;

namespace TraffiLearn.IntegrationTests.Questions.Queries.GetAllQuestions
{
    //TO DO
    public sealed class GetPaginatedQuestionsTests : QuestionIntegrationTest
    {
        public GetPaginatedQuestionsTests(
            WebApplicationFactory factory)
            : base(factory)
        { }

        [Fact]
        public async Task GetPaginatedQuestions_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
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
        public async Task GetPaginatedQuestions_IfNoQuestionsExist_ShouldReturn200StatusCode(
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
        public async Task GetPaginatedQuestions_IfNoQuestionsExist_ShouldReturnEmptyCollection(
            Role eligibleRole)
        {
            var allQuestions = await ApiQuestionClient
                .GetAllQuestionsAsync(
                    getWithRole: eligibleRole);

            allQuestions.Should().NotBeNull();
            allQuestions.Should().BeEmpty();
        }
    }
}
