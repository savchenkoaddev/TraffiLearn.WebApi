using FluentAssertions;
using TraffiLearn.Application.Topics.Queries.GetRandomTopicWithQuestions;
using TraffiLearn.IntegrationTests.Abstractions;
using TraffiLearn.IntegrationTests.Questions;

namespace TraffiLearn.IntegrationTests.Topics.Queries
{
    public sealed class GetRandomTopicWithQuestionsTests : BaseIntegrationTest
    {
        private readonly TopicTestHelper _topicTestHelper;
        private readonly QuestionTestHelper _questionTestHelper;

        public GetRandomTopicWithQuestionsTests(
            WebApplicationFactory factory) 
            : base(factory)
        {
            _topicTestHelper = new(Sender);
            _questionTestHelper = new(Sender);
        }

        [Fact]
        public async Task GetRandomTopicWithQuestions_IfNoTopicsInStorage_ShouldReturnError()
        {
            var result = await Sender.Send(new GetRandomTopicWithQuestionsQuery());

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task GetRandomTopicWithQuestions_IfTopicDoesNotContainQuestions_ShouldBeSuccesful()
        {
            await _topicTestHelper.CreateTopicAsync();

            var result = await Sender.Send(new GetRandomTopicWithQuestionsQuery());

            result.IsSuccess.Should().BeTrue();

            var topic = result.Value;

            topic.Questions.Should().NotBeNull();
            topic.Questions.Should().BeEmpty();
        }

        [Fact]
        public async Task GetRandomTopicWithQuestions_IfValidCase_ShouldBeSuccesful()
        {
            await _topicTestHelper.CreateTopicAsync();

            var firstTopicId = await _topicTestHelper.GetFirstTopicIdAsync();

            await _questionTestHelper.CreateValidQuestionAsync([firstTopicId]);

            var result = await Sender.Send(new GetRandomTopicWithQuestionsQuery());

            result.IsSuccess.Should().BeTrue();

            var topic = result.Value;

            topic.Questions.Should().HaveCount(1);
        }
    }
}
