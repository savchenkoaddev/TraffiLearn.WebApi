using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Questions.AddTopicToQuestion
{
    public sealed record AddTopicToQuestionCommand(
        Guid? TopicId,
        Guid? QuestionId) : IRequest<Result>;
}
