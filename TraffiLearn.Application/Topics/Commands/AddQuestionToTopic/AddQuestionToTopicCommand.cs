using MediatR;

namespace TraffiLearn.Application.Topics.Commands.AddQuestionToTopic
{
    public sealed record AddQuestionToTopicCommand(
        Guid QuestionId,
        Guid TopicId) : IRequest;
}
