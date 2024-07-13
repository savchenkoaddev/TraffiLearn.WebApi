using MediatR;

namespace TraffiLearn.Application.Topics.Commands.RemoveQuestionForTopic
{
    public sealed record RemoveQuestionForTopicCommand(
        Guid? QuestionId,
        Guid? TopicId) : IRequest;
}
