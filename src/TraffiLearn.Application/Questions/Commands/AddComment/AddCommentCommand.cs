using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Questions.Commands.AddComment
{
    public sealed record AddCommentCommand(
        Guid? QuestionId,
        string? Content) : IRequest<Result>;
}
