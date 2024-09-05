using TraffiLearn.Application.Questions.Commands.Create;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;
using TraffiLearn.Testing.Shared.Factories;

namespace TraffiLearn.IntegrationTests.Questions.Commands.CreateQuestion
{
    public sealed class CreateQuestionTests : QuestionIntegrationTest
    {
        private readonly CreateQuestionCommandFactory _commandFactory;

        public CreateQuestionTests(
            WebApplicationFactory factory)
            : base(factory)
        {
            _commandFactory = new(ApiTopicClient);
        }

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
        public async Task CreateQuestion_IfUserIsNotAuthenticated_QuestionShouldNotBeAddedToTopic()
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var topicQuestionsCountBefore = await GetTopicQuestionsCountAsync(topicId);

            await ApiQuestionClient
                .SendValidCreateQuestionRequestAsync(
                    topicIds: [topicId],
                    sentFromRole: null);

            var topicQuestionsCountAfter = await GetTopicQuestionsCountAsync(topicId);

            topicQuestionsCountBefore.Should().Be(topicQuestionsCountAfter);
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task CreateQuestion_IfUserIsNotEligible_ShouldReturn403StatusCode(
            Role nonEligibleRole)
        {
            var response = await ApiQuestionClient
                .SendValidCreateQuestionRequestWithTopicAsync(
                    sentFromRole: nonEligibleRole);

            response.AssertForbiddenStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task CreateQuestion_IfUserIsNotEligible_QuestionShouldNotBeCreated(
            Role nonEligibleRole)
        {
            await ApiQuestionClient
                .SendValidCreateQuestionRequestWithTopicAsync(
                    sentFromRole: nonEligibleRole);

            var allQuestions = await ApiQuestionClient.GetAllQuestionsAsAuthorizedAsync();

            allQuestions.Should().BeEmpty();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task CreateQuestion_IfUserIsNotEligible_QuestionShouldNotBeAddedToTopic(
            Role nonEligibleRole)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var topicQuestionsCountBefore = await GetTopicQuestionsCountAsync(topicId);

            await ApiQuestionClient
                .SendValidCreateQuestionRequestAsync(
                    topicIds: [topicId],
                    sentFromRole: nonEligibleRole);

            var topicQuestionsCountAfter = await GetTopicQuestionsCountAsync(topicId);

            topicQuestionsCountBefore.Should().Be(topicQuestionsCountAfter);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task CreateQuestion_IfPassedInvalidArgs_ShouldReturn400StatusCode(
            Role eligibleRole)
        {
            var invalidCommands = _commandFactory.CreateInvalidCommands();

            foreach (var command in invalidCommands)
            {
                var response = await ApiQuestionClient.SendCreateQuestionRequestAsync(
                    command,
                    sentFromRole: eligibleRole);

                response.AssertBadRequestStatusCode();
            }
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task CreateQuestion_IfPassedInvalidArgs_QuestionShouldNotBeCreated(
            Role eligibleRole)
        {
            var invalidCommands = _commandFactory.CreateInvalidCommands();

            await SendAllCreateQuestionRequestsWithNoImageAsync(
                invalidCommands,
                sentFromRole: eligibleRole);

            var allQuestions = await ApiQuestionClient.GetAllQuestionsAsAuthorizedAsync();

            allQuestions.Should().BeEmpty();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task CreateQuestion_IfPassedInvalidArgs_QuestionShouldNotBeAddedToTopic(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var topicQuestionsCountBefore = await GetTopicQuestionsCountAsync(topicId);

            var invalidCommands = _commandFactory.CreateInvalidCommands(
                topicIds: [topicId]);

            await SendAllCreateQuestionRequestsWithNoImageAsync(
                invalidCommands,
                sentFromRole: eligibleRole);

            var topicQuestionsCountAfter = await GetTopicQuestionsCountAsync(topicId);

            topicQuestionsCountBefore.Should().Be(topicQuestionsCountAfter);
        }

        private async Task<int> GetTopicQuestionsCountAsync(Guid topicId)
        {
            return (await ApiTopicClient.GetTopicQuestionsAsAuthorizedAsync(topicId))
                .Count();
        }

        private async Task SendAllCreateQuestionRequestsWithNoImageAsync(
            IEnumerable<CreateQuestionCommand> requests,
            Role? sentFromRole = null)
        {
            foreach (var command in requests)
            {
                var response = await ApiQuestionClient.SendCreateQuestionRequestAsync(
                    command: command,
                    sentFromRole: sentFromRole);
            }
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task CreateQuestion_IfTopicIsNotFound_ShouldReturn404StatusCode(
            Role eligibleRole)
        {
            var command = _commandFactory.CreateValidCommand(
                topicIds: [Guid.NewGuid()]);

            var response = await ApiQuestionClient.SendCreateQuestionRequestAsync(
                command,
                sentFromRole: eligibleRole);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task CreateQuestion_IfTopicIsNotFound_QuestionShouldNotBeCreated(
            Role eligibleRole)
        {
            var command = _commandFactory.CreateValidCommand(
                topicIds: [Guid.NewGuid()]);

            await ApiQuestionClient.SendCreateQuestionRequestAsync(
                command,
                sentFromRole: eligibleRole);

            var allQuestions = await ApiQuestionClient.GetAllQuestionsAsAuthorizedAsync();

            allQuestions.Should().BeEmpty();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task CreateQuestion_IfValidCase_ShouldReturn204Or201StatusCode(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var command = _commandFactory.CreateValidCommand(
                topicIds: [topicId]);

            var response = await ApiQuestionClient.SendCreateQuestionRequestAsync(
                command,
                sentFromRole: eligibleRole);

            response.AssertCreatedOrNoContentStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task CreateQuestion_IfValidCase_QuestionShouldBeCreated(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var questionId = await ApiQuestionClient.CreateValidQuestionAsync(
                topicIds: [topicId],
                createdWithRole: eligibleRole);

            var allQuestions = await ApiQuestionClient.GetAllQuestionsAsAuthorizedAsync();

            allQuestions.Single().Id.Should().Be(questionId);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task CreateQuestion_IfValidCase_TopicShouldBeAddedToQuestion(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var questionId = await ApiQuestionClient.CreateValidQuestionAsync(
                topicIds: [topicId],
                createdWithRole: eligibleRole);

            var questionTopics = await ApiQuestionClient.GetQuestionTopicsAsAuthorizedUserAsync(
                questionId: questionId);

            questionTopics.Single().Id.Should().Be(topicId);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task CreateQuestion_IfValidCase_QuestionShouldBeAddedToTopic(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var questionId = await ApiQuestionClient.CreateValidQuestionAsync(
                topicIds: [topicId],
                createdWithRole: eligibleRole);

            var topicQuestions = await ApiTopicClient.GetTopicQuestionsAsAuthorizedAsync(
                topicId: topicId);

            topicQuestions.Single().Id.Should().Be(questionId);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task CreateQuestion_IfValidCaseAndImageIsNotProvided_QuestionImageUriShouldBeNull(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var questionId = await ApiQuestionClient.CreateValidQuestionAsync(
                topicIds: [topicId],
                createdWithRole: eligibleRole);

            var allQuestions = await ApiQuestionClient.GetAllQuestionsAsAuthorizedAsync();

            allQuestions.Single().ImageUri.Should().BeNull();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task CreateQuestion_IfValidCaseAndImageIsProvided_QuestionImageUriShouldBeValid(
            Role eligibleRole)
        {
            var image = ImageFixtureFactory.CreateImage();

            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var questionId = await ApiQuestionClient.CreateValidQuestionAsync(
                topicIds: [topicId],
                image: image,
                createdWithRole: eligibleRole);

            var allQuestions = await ApiQuestionClient.GetAllQuestionsAsAuthorizedAsync();

            var imageUriResult = ImageUri.Create(allQuestions.Single().ImageUri);

            imageUriResult.IsSuccess.Should().BeTrue();
        }
    }
}
