using FluentAssertions;
using TraffiLearn.Application.Topics.Queries.GetById;
using TraffiLearn.IntegrationTests.Abstractions;

namespace TraffiLearn.IntegrationTests.Topics.Queries
{
    public sealed class GetTopicByIdTests : BaseIntegrationTest
    {
        private readonly TopicTestHelper _topicTestHelper;

        public GetTopicByIdTests(
            WebApplicationFactory factory) 
            : base(factory)
        {
            _topicTestHelper = new(Sender);
        }

        [Fact]
        public async Task GetTopicById_IfPassedInvalidArgs_ShouldReturnError()
        {
            var result = await Sender.Send(new GetTopicByIdQuery(null));

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task GetTopicById_IfTopicDoesNotExist_ShouldReturnError()
        {
            var result = await Sender.Send(new GetTopicByIdQuery(Guid.NewGuid()));

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task GetTopicById_IfTopicExists_ShouldBeSuccesful()
        {
            await _topicTestHelper.CreateTopicAsync();

            var firstTopicId = await _topicTestHelper.GetFirstTopicIdAsync();

            var result = await Sender.Send(new GetTopicByIdQuery(firstTopicId));

            result.IsSuccess.Should().BeTrue();

            var topic = result.Value;

            topic.Should().NotBeNull();
            topic.TopicId.Should().Be(firstTopicId);
        }
    }
}
