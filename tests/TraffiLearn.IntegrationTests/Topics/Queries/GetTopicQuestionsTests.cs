using FluentAssertions;
using TraffiLearn.Application.Topics.Commands.AddQuestionToTopic;
using TraffiLearn.Application.Topics.Queries.GetTopicQuestions;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Questions;

namespace TraffiLearn.IntegrationTests.Topics.Queries
{
    public sealed class GetTopicQuestionsTests : BaseIntegrationTest
    {
        private readonly TopicTestHelper _topicTestHelper;
        private readonly QuestionTestHelper _questionTestHelper;

        public GetTopicQuestionsTests(
            WebApplicationFactory factory) 
            : base(factory)
        {
            _topicTestHelper = new(Sender);
            _questionTestHelper = new(Sender);
        }

        [Fact]
        public async Task GetTopicQuestions_IfPassedInvalidArgs_ShouldReturnError()
        {
            var result = await Sender.Send(new GetTopicQuestionsQuery(null));

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task GetTopicQuestions_IfTopicDoesNotExist_ShouldReturnError()
        {
            var result = await Sender.Send(new GetTopicQuestionsQuery(Guid.NewGuid()));

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task GetTopicQuestions_IfTopicDoesNotHaveQuestions_ShouldReturnEmptyCollection()
        {
            await _topicTestHelper.CreateTopicAsync();

            var firstTopicId = await _topicTestHelper.GetFirstTopicIdAsync();

            var result = await Sender.Send(new GetTopicQuestionsQuery(firstTopicId));

            result.IsSuccess.Should().BeTrue();

            result.Value.Should().NotBeNull();
            result.Value.Should().BeEmpty();
        }

        [Fact]
        public async Task GetTopicQuestions_IfTopicExists_ShouldBeSuccesful()
        {
            await _topicTestHelper.CreateTopicAsync();

            var firstTopicId = await _topicTestHelper.GetFirstTopicIdAsync();

            await _questionTestHelper.CreateValidQuestionAsync([firstTopicId]);

            var firstQuestionId = await _questionTestHelper.GetFirstQuestionIdAsync();

            await Sender.Send(new AddQuestionToTopicCommand(
                QuestionId: firstQuestionId,
                TopicId: firstTopicId));

            var result = await Sender.Send(new GetTopicQuestionsQuery(firstTopicId));

            result.IsSuccess.Should().BeTrue();

            var topicQuestions = result.Value;

            topicQuestions.Should().HaveCount(1);
            topicQuestions.Any(q => q.Id == firstQuestionId).Should().BeTrue();
        }
    }
}
