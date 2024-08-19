using FluentAssertions;
using TraffiLearn.Application.Topics.Commands.Create;
using TraffiLearn.Application.Topics.Commands.Delete;

namespace TraffiLearn.IntegrationTests.Topics.Commands
{
    public sealed class DeleteTopicTests : BaseIntegrationTest
    {
        private readonly TopicTestHelper _topicTestHelper;

        public DeleteTopicTests(IntegrationTestWebAppFactory factory)
            : base(factory)
        {
            _topicTestHelper = new(Sender);
        }

        [Fact]
        public async Task DeleteTopic_IfPassedInvalidArgs_ShouldReturnError()
        {
            var command = new DeleteTopicCommand(null);

            var result = await Sender.Send(command);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteTopic_IfTopicDoesNotExist_ShouldReturnError()
        {
            await Sender.Send(new CreateTopicCommand(1, "Value"));

            var command = new DeleteTopicCommand(Guid.NewGuid());

            var result = await Sender.Send(command);

            result.IsFailure.Should().BeTrue();

            var topics = await _topicTestHelper.GetAllTopicsAsync();

            topics.Should().HaveCount(1);
        }

        [Fact]
        public async Task DeleteTopic_IfPassedValidArgs_ShouldBeSuccesful()
        {
            await Sender.Send(new CreateTopicCommand(1, "Value"));

            var topicId = await _topicTestHelper.GetFirstTopicIdAsync();

            var command = new DeleteTopicCommand(topicId);

            var result = await Sender.Send(command);

            result.IsSuccess.Should().BeTrue();

            var topics = await _topicTestHelper.GetAllTopicsAsync();

            topics.Should().BeEmpty();
        }
    }
}
