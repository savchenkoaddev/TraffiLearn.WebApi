using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Comments.Commands.Reply
{
    public sealed record ReplyCommand(
        Guid? CommentId,
        string? Content) : IRequest<Result>;
}
