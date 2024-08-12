using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Users.RemoveCommentLike
{
    public sealed record RemoveCommentLikeCommand(
        Guid? CommentId) : IRequest<Result>;
}
