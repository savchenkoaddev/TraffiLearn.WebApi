using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Comments.Reply
{
    public sealed record ReplyCommand(
        Guid? CommentId,
        string? ReplyContent) : IRequest<Result>;
}
