using FluentAssertions;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;

namespace TraffiLearn.IntegrationTests.Questions.Commands.DeleteQuestion
{
    public sealed class DeleteQuestionTests : QuestionIntegrationTest
    {
        public DeleteQuestionTests(
            WebApplicationFactory factory)
            : base(factory)
        { }

        [Fact]
        public async Task DeleteQuestion_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await ApiQuestionClient
                .SendDeleteQuestionRequestAsync(
                    questionId: Guid.NewGuid(),
                    sentFromRole: null);

            response.AssertUnauthorizedStatusCode();
        }

        [Fact]
        public async Task DeleteQuestion_IfUserIsNotAuthenticated_QuestionShouldNotBeDeleted()
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            await ApiQuestionClient
                .SendDeleteQuestionRequestAsync(
                    questionId: questionId,
                    sentFromRole: null);

            var allQuestions = await ApiQuestionClient.GetAllQuestionsAsAuthorizedAsync();

            allQuestions.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task DeleteQuestion_IfUserIsNotEligible_ShouldReturn403StatusCode(
            Role nonEligibleRole)
        {
            var response = await ApiQuestionClient
                .SendDeleteQuestionRequestAsync(
                    questionId: Guid.NewGuid(),
                    sentFromRole: nonEligibleRole);

            response.AssertForbiddenStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task DeleteQuestion_IfUserIsNotEligible_QuestionShouldNotBeDeleted(
           Role nonEligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            await ApiQuestionClient
                .SendDeleteQuestionRequestAsync(
                    questionId: questionId,
                    sentFromRole: nonEligibleRole);

            var allQuestions = await ApiQuestionClient.GetAllQuestionsAsAuthorizedAsync();

            allQuestions.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task DeleteQuestion_IfQuestionIsNotFound_ShouldReturn404StatusCode(
           Role eligibleRole)
        {
            var response = await ApiQuestionClient
                .SendDeleteQuestionRequestAsync(
                    questionId: Guid.NewGuid(),
                    sentFromRole: eligibleRole);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task DeleteQuestion_IfValidCase_ShouldReturn204StatusCode(
          Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var response = await ApiQuestionClient
                .SendDeleteQuestionRequestAsync(
                    questionId: questionId,
                    sentFromRole: eligibleRole);

            response.AssertNoContentStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task DeleteQuestion_IfValidCase_QuestionShouldBeDeleted(
          Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            await ApiQuestionClient
                .SendDeleteQuestionRequestAsync(
                    questionId: questionId,
                    sentFromRole: eligibleRole);

            var allQuestions = await ApiQuestionClient.GetAllQuestionsAsAuthorizedAsync();

            allQuestions.Should().BeEmpty();
        }
    }
}
