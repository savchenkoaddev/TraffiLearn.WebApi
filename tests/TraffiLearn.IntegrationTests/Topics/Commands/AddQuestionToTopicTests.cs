using FluentAssertions;
using TraffiLearn.Application.Questions.Queries.GetQuestionTopics;
using TraffiLearn.Application.Topics.Commands.AddQuestionToTopic;
using TraffiLearn.Application.Topics.Queries.GetTopicQuestions;
using TraffiLearn.IntegrationTests.Questions;

namespace TraffiLearn.IntegrationTests.Topics.Commands
{
    public sealed class AddQuestionToTopicTests : BaseIntegrationTest
    {
        private readonly TopicTestHelper _topicTestHelper;
        private readonly QuestionTestHelper _questionTestHelper;

        public AddQuestionToTopicTests(IntegrationTestWebAppFactory factory)
            : base(factory)
        {
            _topicTestHelper = new(Sender);
            _questionTestHelper = new(Sender);
        }

        [Fact]
        public async Task AddQuestionToTopic_IfPassedInvalidArgs_ShouldReturnError()
        {
            List<AddQuestionToTopicCommand> invalidCommands = [
                new AddQuestionToTopicCommand(null, Guid.NewGuid()),
                new AddQuestionToTopicCommand(Guid.NewGuid(), null)
            ];

            foreach (var command in invalidCommands)
            {
                var result = await Sender.Send(command);

                result.IsFailure.Should().BeTrue();
            }
        }

        [Fact]
        public async Task AddQuestionToTopic_IfQuestionDoesNotExist_ShouldReturnError()
        {
            await _topicTestHelper.CreateTopicAsync();

            var topicId = await _topicTestHelper.GetFirstTopicIdAsync();

            var command = new AddQuestionToTopicCommand(
                Guid.NewGuid(),
                topicId);

            var result = await Sender.Send(command);

            result.IsFailure.Should().BeTrue();

            var topicQuestions = await _topicTestHelper.GetTopicQuestionsAsync(
                topicId);

            topicQuestions.Should().BeEmpty();
        }

        [Fact]
        public async Task AddQuestionToTopic_IfTopicDoesNotExist_ShouldReturnError()
        {
            await InsertValidQuestionWithTopic();

            var questionId = await _questionTestHelper.GetFirstQuestionIdAsync();

            var command = new AddQuestionToTopicCommand(
                QuestionId: questionId,
                TopicId: Guid.NewGuid());

            var result = await Sender.Send(command);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task AddQuestionToTopic_IfQuestionAlreadyAdded_ShouldReturnError()
        {
            var topicId = await InsertValidQuestionWithTopic();

            var questionId = await _questionTestHelper.GetFirstQuestionIdAsync();

            var invalidCommand = new AddQuestionToTopicCommand(
                QuestionId: questionId,
                TopicId: topicId);

            var result = await Sender.Send(invalidCommand);
            var repeatedResult = await Sender.Send(invalidCommand);

            result.IsFailure.Should().BeTrue();
            repeatedResult.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task AddQuestionToTopic_IfPassedValidArgs_ShouldBeSuccesful()
        {
            await InsertValidQuestionWithTopic();

            var questionId = await _questionTestHelper.GetFirstQuestionIdAsync();

            var testTopicTitle = Guid.NewGuid().ToString();

            await _topicTestHelper.CreateTopicAsync(
                number: 1,
                title: testTopicTitle);

            var allTopics = await _topicTestHelper.GetAllTopicsAsync();

            var testTopic = allTopics.First(t => t.Title == testTopicTitle);

            var command = new AddQuestionToTopicCommand(
                QuestionId: questionId,
                TopicId: testTopic.TopicId);

            var result = await Sender.Send(command);

            result.IsSuccess.Should().BeTrue();

            await EnsureQuestionIsAddedToTopic(
                questionId: questionId,
                topicId: testTopic.TopicId);
        }

        private async Task EnsureQuestionIsAddedToTopic(
            Guid questionId,
            Guid topicId)
        {
            var questionTopics = (await Sender.Send(new GetQuestionTopicsQuery(questionId))).Value;

            questionTopics.Any(t => t.TopicId == topicId).Should().BeTrue();

            var topicQuestions = (await Sender.Send(new GetTopicQuestionsQuery(
                TopicId: topicId))).Value;

            topicQuestions.Should().HaveCount(1);
            topicQuestions.Any(q => q.Id == questionId).Should().BeTrue();
        }

        private async Task<Guid> InsertValidQuestionWithTopic()
        {
            await _topicTestHelper.CreateTopicAsync();

            var topicId = await _topicTestHelper.GetFirstTopicIdAsync();

            await _questionTestHelper.CreateValidQuestionAsync(
                TopicIds: [topicId]);

            return topicId;
        }
    }
}
