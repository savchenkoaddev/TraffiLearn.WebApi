using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Comments.DeleteComment
{
    public sealed record DeleteCommentCommand(
        Guid? CommentId) : IRequest<Result>;
}
