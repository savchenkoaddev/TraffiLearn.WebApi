using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Topics.Commands.RemoveQuestionFromTopic
{
    public sealed record RemoveQuestionFromTopicCommand(
        Guid? QuestionId,
        Guid? TopicId) : IRequest<Result>;
}
