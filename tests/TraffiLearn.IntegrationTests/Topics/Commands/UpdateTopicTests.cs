using FluentAssertions;
using TraffiLearn.Application.Topics.Commands.Create;
using TraffiLearn.Application.Topics.Commands.Update;
using TraffiLearn.IntegrationTests.Abstractions;

namespace TraffiLearn.IntegrationTests.Topics.Commands
{
    public sealed class UpdateTopicTests : BaseIntegrationTest
    {
        public UpdateTopicTests(
            WebApplicationFactory factory)
            : base(factory)
        {
        }

        //[Fact]
        //public async Task UpdateTopic_IfPassedInvalidArgs_ShouldReturnError()
        //{
        //    List<UpdateTopicCommand> invalidCommands = CreateInvalidCommands();

        //    foreach (var command in invalidCommands)
        //    {
        //        var result = await Sender.Send(command);

        //        result.IsFailure.Should().BeTrue();
        //    }
        //}

        //[Fact]
        //public async Task UpdateTopic_IfTopicDoesNotExist_ShouldReturnError()
        //{
        //    var command = new UpdateTopicCommand(Guid.NewGuid(), 1, "Value");

        //    var result = await Sender.Send(command);

        //    result.IsFailure.Should().BeTrue();
        //}

        //[Fact]
        //public async Task UpdateTopic_IfPassedValidArgs_ShouldBeSuccesful()
        //{
        //    await Sender.Send(new CreateTopicCommand(1, "Value"));

        //    var topicId = await _topicTestHelper.GetFirstTopicIdAsync();

        //    var newNumber = 2;
        //    var newTitle = "Value-updated";

        //    var command = new UpdateTopicCommand(topicId, newNumber, newTitle);

        //    var result = await Sender.Send(command);

        //    result.IsSuccess.Should().BeTrue();

        //    await EnsureTopicIsUpdated(topicId, newNumber, newTitle);
        //}

        //private async Task EnsureTopicIsUpdated(
        //    Guid topicId,
        //    int newNumber,
        //    string newTitle)
        //{
        //    var topics = await _topicTestHelper.GetAllTopicsAsync();

        //    topics.Should().HaveCount(1);

        //    var updatedTopic = topics.First();

        //    updatedTopic.TopicNumber.Should().Be(newNumber);
        //    updatedTopic.Title.Should().Be(newTitle);
        //    updatedTopic.TopicId.Should().Be(topicId);
        //}

        //private static List<UpdateTopicCommand> CreateInvalidCommands()
        //{
        //    return [
        //        new UpdateTopicCommand(null, 1, "Value"),
        //        new UpdateTopicCommand(Guid.NewGuid(), null, "Value"),
        //        new UpdateTopicCommand(Guid.NewGuid(), 1, null),
        //    ];
        //}
    }
}
