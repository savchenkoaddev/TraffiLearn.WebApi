using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Comments.Commands.DeleteComment
{
    public sealed record DeleteCommentCommand(
        Guid? CommentId) : IRequest<Result>;
}
