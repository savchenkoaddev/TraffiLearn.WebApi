using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Questions.RemoveTopicFromQuestion
{
    public sealed record RemoveTopicFromQuestionCommand(
        Guid? TopicId,
        Guid? QuestionId) : IRequest<Result>;
}
