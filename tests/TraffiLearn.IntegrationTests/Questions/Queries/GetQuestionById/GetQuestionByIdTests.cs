using TraffiLearn.Domain.Users.Roles;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;

namespace TraffiLearn.IntegrationTests.Questions.Queries.GetQuestionById
{
    public sealed class GetQuestionByIdTests : QuestionIntegrationTest
    {
        public GetQuestionByIdTests(
            WebApplicationFactory factory)
            : base(factory)
        { }

        [Fact]
        public async Task GetQuestionById_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await ApiQuestionClient
                .SendGetQuestionByIdRequestAsync(
                    questionId: Guid.NewGuid(),
                    sentFromRole: null);

            response.AssertUnauthorizedStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetQuestionById_IfQuestionIsNotFound_ShouldReturn404StatusCode(
            Role eligibleRole)
        {
            var response = await ApiQuestionClient.SendGetQuestionByIdRequestAsync(
                questionId: Guid.NewGuid(),
                sentFromRole: eligibleRole);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetQuestionById_IfValidCase_ShouldReturn200StatusCode(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var response = await ApiQuestionClient.SendGetQuestionByIdRequestAsync(
                questionId: questionId,
                sentFromRole: eligibleRole);

            response.AssertOkStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task GetQuestionById_IfValidCase_ShouldReturnValidQuestion(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var question = await ApiQuestionClient.GetQuestionByIdAsync(
                questionId: questionId,
                getWithRole: eligibleRole);

            question.Id.Should().Be(questionId);
        }
    }
}
