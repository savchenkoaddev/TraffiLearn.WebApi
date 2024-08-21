using FluentAssertions;
using TraffiLearn.Application.Topics.Queries.GetRandomTopicWithQuestions;
using TraffiLearn.IntegrationTests.Abstractions;

namespace TraffiLearn.IntegrationTests.Topics.Queries
{
    public sealed class GetRandomTopicWithQuestionsTests : BaseIntegrationTest
    {
        public GetRandomTopicWithQuestionsTests(
            WebApplicationFactory factory)
            : base(factory)
        {
        }

        //[Fact]
        //public async Task GetRandomTopicWithQuestions_IfNoTopicsInStorage_ShouldReturnError()
        //{
        //    var result = await Sender.Send(new GetRandomTopicWithQuestionsQuery());

        //    result.IsFailure.Should().BeTrue();
        //}

        //[Fact]
        //public async Task GetRandomTopicWithQuestions_IfTopicDoesNotContainQuestions_ShouldBeSuccesful()
        //{
        //    await _topicTestHelper.CreateTopicAsync();

        //    var result = await Sender.Send(new GetRandomTopicWithQuestionsQuery());

        //    result.IsSuccess.Should().BeTrue();

        //    var topic = result.Value;

        //    topic.Questions.Should().NotBeNull();
        //    topic.Questions.Should().BeEmpty();
        //}

        //[Fact]
        //public async Task GetRandomTopicWithQuestions_IfValidCase_ShouldBeSuccesful()
        //{
        //    await _topicTestHelper.CreateTopicAsync();

        //    var firstTopicId = await _topicTestHelper.GetFirstTopicIdAsync();

        //    await _questionTestHelper.CreateValidQuestionAsync([firstTopicId]);

        //    var result = await Sender.Send(new GetRandomTopicWithQuestionsQuery());

        //    result.IsSuccess.Should().BeTrue();

        //    var topic = result.Value;

        //    topic.Questions.Should().HaveCount(1);
        //}
    }
}
