using MediatR;

namespace TraffiLearn.Application.Commands.Topics.AddQuestionToTopic
{
    public sealed record AddQuestionToTopicCommand(
        Guid QuestionId,
        Guid TopicId) : IRequest;
}
