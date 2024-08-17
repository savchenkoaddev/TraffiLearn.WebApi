using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Users.Commands.DislikeComment
{
    public sealed record DislikeCommentCommand(
        Guid? CommentId) : IRequest<Result>;
}
