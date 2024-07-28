using MediatR;

namespace TraffiLearn.Application.Commands.Topics.RemoveQuestionForTopic
{
    public sealed record RemoveQuestionForTopicCommand(
        Guid? QuestionId,
        Guid? TopicId) : IRequest;
}
