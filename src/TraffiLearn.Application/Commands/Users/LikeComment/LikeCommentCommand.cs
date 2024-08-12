using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Users.LikeComment
{
    public sealed record LikeCommentCommand(
        Guid? CommentId) : IRequest<Result>;
}
