using FluentAssertions;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;

namespace TraffiLearn.IntegrationTests.Questions.Commands.UpdateQuestion
{
    public sealed class UpdateQuestionTests : QuestionIntegrationTest
    {
        private readonly UpdateQuestionCommandFactory _commandFactory = new();

        public UpdateQuestionTests(
            WebApplicationFactory factory)
            : base(factory)
        { }

        [Fact]
        public async Task UpdateQuestion_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await ApiQuestionClient
                .SendValidUpdateQuestionRequestAsync(
                    questionId: Guid.NewGuid(),
                    topicIds: [Guid.NewGuid()],
                    sentFromRole: null);

            response.AssertUnauthorizedStatusCode();
        }

        [Fact]
        public async Task UpdateQuestion_IfUserIsNotAuthenticated_QuestionShouldNotBeUpdated()
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var updatedExplanation = "updated-explanation";

            var command = _commandFactory.CreateValidCommand(
                questionId,
                topicIds: [Guid.NewGuid()]) with
            { Explanation = updatedExplanation };

            await ApiQuestionClient
                .SendUpdateQuestionRequestAsync(
                    request: command,
                    sentFromRole: null);

            var allQuestions = await ApiQuestionClient.GetAllQuestionsAsAuthorizedAsync();

            allQuestions.Single().Explanation.Should().NotBe(updatedExplanation);
        }
    }
}
