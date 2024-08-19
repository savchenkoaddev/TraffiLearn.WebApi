using FluentAssertions;
using TraffiLearn.Application.Questions.Queries.GetTopicQuestions;
using TraffiLearn.Application.Topics.Commands.AddQuestionToTopic;
using TraffiLearn.Application.Topics.Commands.RemoveQuestionFromTopic;
using TraffiLearn.Application.Topics.Queries.GetQuestionTopics;
using TraffiLearn.IntegrationTests.Questions;

namespace TraffiLearn.IntegrationTests.Topics.Commands
{
    public sealed class RemoveQuestionFromTopicTests : BaseIntegrationTest
    {
        private readonly TopicTestHelper _topicTestHelper;
        private readonly QuestionTestHelper _questionTestHelper;

        public RemoveQuestionFromTopicTests(IntegrationTestWebAppFactory factory)
            : base(factory)
        {
            _topicTestHelper = new(Sender);
            _questionTestHelper = new(Sender);
        }

        [Fact]
        public async Task RemoveQuestionFromTopic_IfPassedInvalidArgs()
        {
            List<RemoveQuestionFromTopicCommand> invalidCommands = [
                new RemoveQuestionFromTopicCommand(null, Guid.NewGuid()),
                new RemoveQuestionFromTopicCommand(Guid.NewGuid(), null)
            ];

            foreach (var command in invalidCommands)
            {
                var result = await Sender.Send(command);

                result.IsFailure.Should().BeTrue();
            }
        }

        [Fact]
        public async Task RemoveQuestionFromTopic_IfQuestionDoesNotExist_ShouldReturnError()
        {
            await _topicTestHelper.CreateValidTopicAsync();

            var topicId = await _topicTestHelper.GetFirstTopicIdAsync();

            var command = new RemoveQuestionFromTopicCommand(Guid.NewGuid(), topicId);

            var result = await Sender.Send(command);

            result.IsFailure.Should().BeTrue();

            var topicQuestions = await _topicTestHelper.GetTopicQuestionsAsync(topicId);

            topicQuestions.Should().BeEmpty();
        }

        [Fact]
        public async Task RemoveQuestionFromTopic_IfTopicDoesNotExist_ShouldReturnError()
        {
            await _topicTestHelper.CreateValidTopicAsync();

            var topicId = await _topicTestHelper.GetFirstTopicIdAsync();

            await _questionTestHelper.CreateValidQuestionAsync(
                TopicIds: [topicId]);

            var questionId = await _questionTestHelper.GetFirstQuestionIdAsync();

            var command = new RemoveQuestionFromTopicCommand(
                QuestionId: questionId,
                TopicId: Guid.NewGuid());

            var result = await Sender.Send(command);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task RemoveQuestionFromTopic_IfTopicDoesNotContainQuestion_ShouldReturnError()
        {
            await InsertValidQuestionWithTopic();

            var firstQuestionId = await _questionTestHelper.GetFirstQuestionIdAsync();

            var testTopicTitle = Guid.NewGuid();

            await _topicTestHelper.CreateValidTopicAsync(
                number: 1,
                title: testTopicTitle.ToString());

            var allTopics = await _topicTestHelper.GetAllTopicsAsync();

            var testTopic = allTopics.First(
                t => t.Title == testTopicTitle.ToString());

            var result = await Sender.Send(new RemoveQuestionFromTopicCommand(
                QuestionId: firstQuestionId,
                TopicId: testTopic.TopicId));

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task RemoveQuestionFromTopic_IfPassedValidArgs_ShouldBeSuccesful()
        {
            await InsertValidQuestionWithTopic();

            var testTopicTitle = Guid.NewGuid().ToString();

            await _topicTestHelper.CreateValidTopicAsync(
                number: 1,
                title: testTopicTitle);

            var questionId = await _questionTestHelper.GetFirstQuestionIdAsync();

            var allTopics = await _topicTestHelper.GetAllTopicsAsync();

            var testTopic = allTopics.First(t => t.Title == testTopicTitle);

            await Sender.Send(new AddQuestionToTopicCommand(
                questionId,
                testTopic.TopicId));

            var command = new RemoveQuestionFromTopicCommand(
                QuestionId: questionId,
                TopicId: testTopic.TopicId);

            var result = await Sender.Send(command);

            result.IsSuccess.Should().BeTrue();

            await EnsureQuestionIsRemovedFromTopic(
                questionId,
                testTopic.TopicId);
        }

        private async Task EnsureQuestionIsRemovedFromTopic(
            Guid questionId,
            Guid topicId)
        {
            var questionTopics = (await Sender.Send(new GetQuestionTopicsQuery(questionId))).Value;

            questionTopics.Should().HaveCount(1);
            questionTopics.Any(t => t.TopicId == topicId).Should().BeFalse();

            var topicQuestions = (await Sender.Send(new GetTopicQuestionsQuery(
                TopicId: topicId))).Value;

            topicQuestions.Should().BeEmpty();
            topicQuestions.Any(q => q.Id == questionId).Should().BeFalse();
        }

        private async Task<Guid> InsertValidQuestionWithTopic()
        {
            await _topicTestHelper.CreateValidTopicAsync();

            var topicId = await _topicTestHelper.GetFirstTopicIdAsync();

            await _questionTestHelper.CreateValidQuestionAsync(
                TopicIds: [topicId]);

            return topicId;
        }
    }
}
