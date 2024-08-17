using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Users.Commands.LikeComment
{
    public sealed record LikeCommentCommand(
        Guid? CommentId) : IRequest<Result>;
}
