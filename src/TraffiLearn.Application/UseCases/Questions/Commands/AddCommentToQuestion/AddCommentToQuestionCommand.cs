using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Questions.Commands.AddCommentToQuestion
{
    public sealed record AddCommentToQuestionCommand(
        Guid QuestionId,
        string Content) : IRequest<Result>;
}
