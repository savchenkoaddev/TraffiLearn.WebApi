using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Comments.Commands.Reply
{
    public sealed record ReplyCommand(
        Guid? CommentId,
        string? Content) : IRequest<Result>;
}
