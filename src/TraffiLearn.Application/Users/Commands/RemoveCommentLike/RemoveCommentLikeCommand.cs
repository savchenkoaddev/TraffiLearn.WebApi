using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Users.Commands.RemoveCommentLike
{
    public sealed record RemoveCommentLikeCommand(
        Guid? CommentId) : IRequest<Result>;
}
