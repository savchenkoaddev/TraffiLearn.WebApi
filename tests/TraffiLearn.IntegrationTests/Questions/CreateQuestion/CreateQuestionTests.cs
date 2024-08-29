using FluentAssertions;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;

namespace TraffiLearn.IntegrationTests.Questions.CreateQuestion
{
    public sealed class CreateQuestionTests : QuestionIntegrationTest
    {
        public CreateQuestionTests(
            WebApplicationFactory factory)
            : base(factory)
        { }

        [Fact]
        public async Task CreateQuestion_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await ApiQuestionClient
                .SendValidCreateQuestionRequestWithTopicAsync(
                    sentFromRole: null);

            response.AssertUnauthorizedStatusCode();
        }

        [Fact]
        public async Task CreateQuestion_IfUserIsNotAuthenticated_QuestionShouldNotBeCreated()
        {
            await ApiQuestionClient
                .SendValidCreateQuestionRequestWithTopicAsync(
                    sentFromRole: null);

            var allQuestions = await ApiQuestionClient.GetAllQuestionsAsAuthorizedAsync();

            allQuestions.Should().BeEmpty();
        }

        [Fact]
        public async Task CreateQuestion_IfUserIsNotAuthenticated_ImageShouldNotBeUploaded()
        {
            await ApiQuestionClient
                .SendValidCreateQuestionRequestWithTopicAsync(
                    sentFromRole: null);

            var allQuestions = await ApiQuestionClient.GetAllQuestionsAsAuthorizedAsync();

            allQuestions.Should().BeEmpty();
        }
    }
}
