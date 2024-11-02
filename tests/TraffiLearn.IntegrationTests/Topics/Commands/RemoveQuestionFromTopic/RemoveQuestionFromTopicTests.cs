using TraffiLearn.Domain.Users.Roles;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Extensions;
using TraffiLearn.IntegrationTests.Questions;

namespace TraffiLearn.IntegrationTests.Topics.Commands.RemoveQuestionFromTopic
{
    public sealed class RemoveQuestionFromTopicTests : TopicIntegrationTest
    {
        public RemoveQuestionFromTopicTests(
            WebApplicationFactory factory)
            : base(factory)
        { }

        [Fact]
        public async Task RemoveQuestionFromTopic_IfUserIsNotAuthenticated_ShouldReturn401StatusCode()
        {
            var response = await ApiTopicClient.SendRemoveQuestionFromTopicRequestAsync(
                questionId: Guid.NewGuid(),
                topicId: Guid.NewGuid());

            response.AssertUnauthorizedStatusCode();
        }

        [Fact]
        public async Task RemoveQuestionFromTopic_IfUserIsNotAuthenticated_QuestionShouldNotBeRemovedFromTopic()
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var questionId = await ApiQuestionClient.CreateValidQuestionAsAuthorizedAsync(
                topicIds: [topicId]);

            await ApiTopicClient.SendRemoveQuestionFromTopicRequestAsync(
                questionId: questionId,
                topicId: topicId);

            var topicQuestions = await ApiTopicClient.GetTopicQuestionsAsAuthorizedAsync(
                topicId);

            topicQuestions.Should().NotBeEmpty();
            topicQuestions.Single().Id.Should().Be(questionId);
        }

        [Fact]
        public async Task RemoveQuestionFromTopic_IfUserIsNotAuthenticated_TopicShouldNotBeRemovedFromQuestion()
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var questionId = await ApiQuestionClient.CreateValidQuestionAsAuthorizedAsync(
                topicIds: [topicId]);

            await ApiTopicClient.SendRemoveQuestionFromTopicRequestAsync(
                questionId: questionId,
                topicId: topicId);

            var topicQuestions = await ApiQuestionClient.GetQuestionTopicsAsAuthorizedUserAsync(
                questionId);

            topicQuestions.Should().NotBeEmpty();
            topicQuestions.Single().Id.Should().Be(topicId);
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task RemoveQuestionFromTopic_IfUserIsNotEligible_ShouldReturn403StatusCode(
            Role nonEligibleRole)
        {
            var response = await ApiTopicClient.SendRemoveQuestionFromTopicRequestAsync(
                questionId: Guid.NewGuid(),
                topicId: Guid.NewGuid(),
                sentFromRole: nonEligibleRole);

            response.AssertForbiddenStatusCode();
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task RemoveQuestionFromTopic_IfUserIsNotEligible_QuestionShouldNotBeRemovedFromTopic(
            Role nonEligibleRole)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var questionId = await ApiQuestionClient.CreateValidQuestionAsAuthorizedAsync(
                topicIds: [topicId]);

            var response = await ApiTopicClient.SendRemoveQuestionFromTopicRequestAsync(
                questionId: questionId,
                topicId: topicId,
                sentFromRole: nonEligibleRole);

            var topicQuestions = await ApiTopicClient.GetTopicQuestionsAsAuthorizedAsync(
                topicId);

            topicQuestions.Should().NotBeEmpty();
            topicQuestions.Single().Id.Should().Be(questionId);
        }

        [Theory]
        [InlineData(Role.RegularUser)]
        public async Task RemoveQuestionFromTopic_IfUserIsNotEligible_TopicShouldNotBeRemovedFromQuestion(
           Role nonEligibleRole)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var questionId = await ApiQuestionClient.CreateValidQuestionAsAuthorizedAsync(
                topicIds: [topicId]);

            await ApiTopicClient.SendRemoveQuestionFromTopicRequestAsync(
                questionId: questionId,
                topicId: topicId,
                sentFromRole: nonEligibleRole);

            var questionTopics = await ApiQuestionClient.GetQuestionTopicsAsAuthorizedUserAsync(
                questionId);

            questionTopics.Should().NotBeEmpty();
            questionTopics.Single().Id.Should().Be(topicId);
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task RemoveQuestionFromTopic_IfQuestionIsNotFound_ShouldReturn404StatusCode(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var response = await ApiTopicClient.SendRemoveQuestionFromTopicRequestAsync(
                questionId: Guid.NewGuid(),
                topicId: topicId,
                sentFromRole: eligibleRole);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task RemoveQuestionFromTopic_IfTopicIsNotFound_ShouldReturn404StatusCode(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var questionId = await ApiQuestionClient.CreateValidQuestionAsAuthorizedAsync(
                topicIds: [topicId]);

            var response = await ApiTopicClient.SendRemoveQuestionFromTopicRequestAsync(
                questionId: questionId,
                topicId: Guid.NewGuid(),
                sentFromRole: eligibleRole);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task RemoveQuestionFromTopic_IfQuestionIsNotAdded_ShouldReturn404StatusCode(
            Role eligibleRole)
        {
            var questionId = await ApiQuestionClient
                .CreateValidQuestionWithTopicAsAuthorizedAsync();

            var newTopicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var response = await ApiTopicClient.SendRemoveQuestionFromTopicRequestAsync(
                questionId: questionId,
                topicId: newTopicId,
                sentFromRole: eligibleRole);

            response.AssertNotFoundStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task RemoveQuestionFromTopic_IfValidCase_ShouldReturn204StatusCode(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var questionId = await ApiQuestionClient.CreateValidQuestionAsAuthorizedAsync(
                topicIds: [topicId]);

            var response = await ApiTopicClient.SendRemoveQuestionFromTopicRequestAsync(
                questionId: questionId,
                topicId: topicId,
                sentFromRole: eligibleRole);

            response.AssertNoContentStatusCode();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task RemoveQuestionFromTopic_IfValidCase_QuestionShouldBeRemovedFromTopic(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var questionId = await ApiQuestionClient.CreateValidQuestionAsAuthorizedAsync(
                topicIds: [topicId]);

            var response = await ApiTopicClient.SendRemoveQuestionFromTopicRequestAsync(
                questionId: questionId,
                topicId: topicId,
                sentFromRole: eligibleRole);

            var topicQuestions = await ApiTopicClient
                .GetTopicQuestionsAsAuthorizedAsync(topicId);

            topicQuestions.Should().BeEmpty();
        }

        [Theory]
        [InlineData(Role.Admin)]
        [InlineData(Role.Owner)]
        public async Task RemoveQuestionFromTopic_IfValidCase_TopicShouldBeRemovedFromQuestion(
            Role eligibleRole)
        {
            var topicId = await ApiTopicClient.CreateValidTopicAsAuthorizedAsync();

            var questionId = await ApiQuestionClient.CreateValidQuestionAsAuthorizedAsync(
                topicIds: [topicId]);

            var response = await ApiTopicClient.SendRemoveQuestionFromTopicRequestAsync(
                questionId: questionId,
                topicId: topicId,
                sentFromRole: eligibleRole);

            var questionTopics = await ApiQuestionClient
                .GetQuestionTopicsAsAuthorizedUserAsync(questionId);

            questionTopics.Should().BeEmpty();
        }
    }
}
