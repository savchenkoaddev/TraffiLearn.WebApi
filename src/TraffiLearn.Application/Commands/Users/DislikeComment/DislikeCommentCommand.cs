using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Users.DislikeComment
{
    public sealed record DislikeCommentCommand(
        Guid? CommentId) : IRequest<Result>;
}
