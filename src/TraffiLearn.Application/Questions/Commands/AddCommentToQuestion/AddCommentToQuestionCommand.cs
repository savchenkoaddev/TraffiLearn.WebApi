using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Questions.Commands.AddCommentToQuestion
{
    public sealed record AddCommentToQuestionCommand(
        Guid? QuestionId,
        string? Content) : IRequest<Result>;
}
