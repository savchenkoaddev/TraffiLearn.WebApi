using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Topics.RemoveQuestionFromTopic
{
    public sealed record RemoveQuestionFromTopicCommand(
        Guid? QuestionId,
        Guid? TopicId) : IRequest<Result>;
}
