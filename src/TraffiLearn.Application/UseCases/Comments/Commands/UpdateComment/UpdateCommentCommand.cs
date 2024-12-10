using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Comments.Commands.UpdateComment
{
    public sealed record UpdateCommentCommand(
        Guid? CommentId,
        string? Content) : IRequest<Result>;
}
