using FluentAssertions;
using TraffiLearn.Application.Topics.Commands.Create;
using TraffiLearn.Application.Topics.DTO;
using TraffiLearn.Testing.Shared.Factories;

namespace TraffiLearn.IntegrationTests.Topics.Commands
{
    public sealed class CreateTopicTests : BaseIntegrationTest
    {
        private readonly TopicTestHelper _topicTestHelper;

        public CreateTopicTests(IntegrationTestWebAppFactory factory)
            : base(factory)
        {
            _topicTestHelper = new TopicTestHelper(Sender);
        }

        [Fact]
        public async Task CreateTopic_IfPassedInvalidArgs_ShouldReturnError()
        {
            List<CreateTopicCommand> invalidCommands = CreateInvalidCommands();

            foreach (var command in invalidCommands)
            {
                var result = await Sender.Send(command);

                result.IsFailure.Should().BeTrue();

                var allTopics = await _topicTestHelper.GetAllTopicsAsync();

                allTopics.Should().BeEmpty();
            }
        }

        [Fact]
        public async Task CreateTopic_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var topic = TopicFixtureFactory.CreateTopic();

            var command = new CreateTopicCommand(
                topic.Number.Value,
                topic.Title.Value);

            var result = await Sender.Send(command);

            result.IsSuccess.Should().BeTrue();

            var topics = await _topicTestHelper.GetAllTopicsAsync();

            topics.Should().HaveCount(1);

            var newTopic = topics.First();

            EnsureTopicCreatedCorrectly(command, newTopic);
        }

        private static List<CreateTopicCommand> CreateInvalidCommands()
        {
            return [
                new CreateTopicCommand(-1, ""),
                new CreateTopicCommand(1, ""),
                new CreateTopicCommand(-1, "Value"),
                new CreateTopicCommand(null, "Value"),
                new CreateTopicCommand(1, null),
            ];
        }

        private static void EnsureTopicCreatedCorrectly(
            CreateTopicCommand command,
            TopicResponse newTopic)
        {
            newTopic.Title.Should().Be(command.Title);
            newTopic.TopicNumber.Should().Be(command.TopicNumber!.Value);
            newTopic.TopicId.Should().NotBe(Guid.Empty);
        }
    }
}

