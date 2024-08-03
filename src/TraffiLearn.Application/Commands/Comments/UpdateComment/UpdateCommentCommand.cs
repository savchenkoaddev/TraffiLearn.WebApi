using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Comments.UpdateComment
{
    public sealed record UpdateCommentCommand(
        Guid? CommentId,
        string? Content) : IRequest<Result>;
}
