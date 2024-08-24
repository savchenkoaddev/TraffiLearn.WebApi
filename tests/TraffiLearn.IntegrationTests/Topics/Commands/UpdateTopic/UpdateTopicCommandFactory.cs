using TraffiLearn.Application.Topics.Commands.Update;

namespace TraffiLearn.IntegrationTests.Topics.Commands.UpdateTopic
{
    internal static class UpdateTopicCommandFactory
    {
        public static List<UpdateTopicCommand> CreateInvalidCommandsWithRandomIds()
        {
            return [
                new UpdateTopicCommand(
                    TopicId: null,
                    TopicNumber: 1,
                    Title: "title"),

                new UpdateTopicCommand(
                    TopicId: Guid.NewGuid(),
                    TopicNumber: -1,
                    Title: "title"),

                new UpdateTopicCommand(
                    TopicId: Guid.NewGuid(),
                    TopicNumber: null,
                    Title: "title"),

                new UpdateTopicCommand(
                    TopicId: Guid.NewGuid(),
                    TopicNumber: 1,
                    Title: null),

                new UpdateTopicCommand(
                    TopicId: Guid.NewGuid(),
                    TopicNumber: 1,
                    Title: string.Empty),

                new UpdateTopicCommand(
                    TopicId: Guid.NewGuid(),
                    TopicNumber: 1,
                    Title: " ")
            ];
        }

        public static UpdateTopicCommand CreateValidCommand(Guid topicId)
        {
            return new UpdateTopicCommand(
                TopicId: topicId,
                TopicNumber: 2,
                Title: "updated-title");
        }

        public static UpdateTopicCommand CreateValidCommandWithRandomId()
        {
            return new UpdateTopicCommand(
                TopicId: Guid.NewGuid(),
                TopicNumber: 2,
                Title: "updated-title");
        }
    }
}
