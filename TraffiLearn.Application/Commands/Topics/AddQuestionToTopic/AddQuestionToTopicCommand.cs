using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Topics.AddQuestionToTopic
{
    public sealed record AddQuestionToTopicCommand(
        Guid? QuestionId,
        Guid? TopicId) : IRequest<Result>;
}
