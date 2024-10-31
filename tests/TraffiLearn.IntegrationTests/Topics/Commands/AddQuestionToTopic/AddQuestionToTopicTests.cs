using TraffiLearn.Domain.Aggregates.Users.Roles;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;
using TraffiLearn.IntegrationTests.Questions;

namespace TraffiLearn.IntegrationTests.Topics.Commands.AddQuestionToTopic
{
    public sealed class AddQuestionToTopicTests : TopicIntegrationTest
    {
        public AddQuestionToTopicTests(
            WebApplicationFactory factory)
            : base(factory)
        { }

        [Fact]
        public async Task AddQuestionToTopic_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await ApiTopicClient.SendAddQuestionToTopicRequestAsync(
                questionId: Guid.NewGuid(),
                topicId: Guid.NewGuid());

            response.AssertUnauthorizedStatusCode();
        }

        [Fact]
        public async Task AddQuestionToTopic_IfUserIsNotAuthenticated_QuestionShouldNotBeAddedToTopic()
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            await ApiTopicClient.SendAddQuestionToTopicRequestAsync(
                questionId: questionId,
                topicId: topicId);

            var topicQuestions = await ApiTopicClient.GetTopicQuestionsAsAuthorizedAsync(
                topicId);

            topicQuestions.Any(q => q.Id == questionId).Should().BeFalse();
        }

        [Fact]
        public async Task AddQuestionToTopic_IfUserIsNotAuthenticated_TopicShouldNotBeAddedToQuestion()
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            await ApiTopicClient.SendAddQuestionToTopicRequestAsync(
                questionId: questionId,
                topicId: topicId);

            var questionTopics = await ApiQuestionClient.GetQuestionTopicsAsAuthorizedUserAsync(
                questionId);

            questionTopics.Any(q => q.Id == topicId).Should().BeFalse();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task AddQuestionToTopic_IfUserIsNotEligible_ShouldReturn403StatusCode(
            Role nonEligibleRole)
        {
            var response = await ApiTopicClient.SendAddQuestionToTopicRequestAsync(
                questionId: Guid.NewGuid(),
                topicId: Guid.NewGuid(),
                sentFromRole: nonEligibleRole);

            response.AssertForbiddenStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task AddQuestionToTopic_IfUserIsNotEligible_QuestionShouldNotBeAddedToTopic(
            Role nonEligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            await ApiTopicClient.SendAddQuestionToTopicRequestAsync(
                questionId: questionId,
                topicId: topicId,
                sentFromRole: nonEligibleRole);

            var topicQuestions = await ApiTopicClient.GetTopicQuestionsAsAuthorizedAsync(
                topicId);

            topicQuestions.Any(q => q.Id == questionId).Should().BeFalse();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task AddQuestionToTopic_IfUserIsNotEligible_TopicShouldNotBeAddedToQuestion(
           Role nonEligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            await ApiTopicClient.SendAddQuestionToTopicRequestAsync(
                questionId: questionId,
                topicId: topicId,
                sentFromRole: nonEligibleRole);

            var questionTopics = await ApiQuestionClient.GetQuestionTopicsAsAuthorizedUserAsync(
                questionId);

            questionTopics.Any(q => q.Id == topicId).Should().BeFalse();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task AddQuestionToTopic_IfQuestionIsNotFound_ShouldReturn404StatusCode(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var response = await ApiTopicClient.SendAddQuestionToTopicRequestAsync(
                questionId: Guid.NewGuid(),
                topicId: topicId,
                sentFromRole: eligibleRole);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task AddQuestionToTopic_IfTopicIsNotFound_ShouldReturn404StatusCode(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var response = await ApiTopicClient.SendAddQuestionToTopicRequestAsync(
                questionId: questionId,
                topicId: Guid.NewGuid(),
                sentFromRole: eligibleRole);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task AddQuestionToTopic_IfQuestionAlreadyAdded_ShouldReturn400StatusCode(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var questionId = await ApiQuestionClient.CreateValidQuestionAsAuthorizedAsync(
                topicIds: [topicId]);

            var response = await ApiTopicClient.SendAddQuestionToTopicRequestAsync(
                questionId: questionId,
                topicId: topicId,
                sentFromRole: eligibleRole);

            response.AssertBadRequestStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task AddQuestionToTopic_IfQuestionAlreadyAdded_DuplicateQuestionShouldNotBeAddedToTopic(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var questionId = await ApiQuestionClient.CreateValidQuestionAsAuthorizedAsync(
                topicIds: [topicId]);

            await ApiTopicClient.SendAddQuestionToTopicRequestAsync(
                questionId: questionId,
                topicId: topicId,
                sentFromRole: eligibleRole);

            var topicQuestions = await ApiTopicClient.GetTopicQuestionsAsAuthorizedAsync(
                topicId);

            topicQuestions.Should().HaveCount(1);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task AddQuestionToTopic_IfQuestionAlreadyAdded_DuplicateTopicShouldNotBeAddedToQuestion(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var questionId = await ApiQuestionClient.CreateValidQuestionAsAuthorizedAsync(
                topicIds: [topicId]);

            await ApiTopicClient.SendAddQuestionToTopicRequestAsync(
                questionId: questionId,
                topicId: topicId,
                sentFromRole: eligibleRole);

            var questionTopics = await ApiQuestionClient.GetQuestionTopicsAsAuthorizedUserAsync(
                questionId);

            questionTopics.Should().HaveCount(1);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task AddQuestionToTopic_IfValidCase_ShouldReturn204StatusCode(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var newTopicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var response = await ApiTopicClient.SendAddQuestionToTopicRequestAsync(
                questionId: questionId,
                topicId: newTopicId,
                sentFromRole: eligibleRole);

            response.AssertNoContentStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task AddQuestionToTopic_IfValidCase_QuestionShouldBeAddedToTopic(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var newTopicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            await ApiTopicClient.SendAddQuestionToTopicRequestAsync(
                questionId: questionId,
                topicId: newTopicId,
                sentFromRole: eligibleRole);

            var topicQuestions = await ApiTopicClient.GetTopicQuestionsAsAuthorizedAsync(
                newTopicId);

            topicQuestions.Single().Id.Should().Be(questionId);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task AddQuestionToTopic_IfValidCase_TopicShouldBeAddedToQuestion(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var newTopicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            await ApiTopicClient.SendAddQuestionToTopicRequestAsync(
               questionId: questionId,
               topicId: newTopicId,
               sentFromRole: eligibleRole);

            var questionTopics = await ApiQuestionClient.GetQuestionTopicsAsAuthorizedUserAsync(questionId);

            questionTopics.Any(t => t.Id == newTopicId).Should().BeTrue();
        }
    }
}
