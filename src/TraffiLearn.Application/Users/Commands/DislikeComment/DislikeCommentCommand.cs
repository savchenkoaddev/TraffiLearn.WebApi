using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Users.Commands.DislikeComment
{
    public sealed record DislikeCommentCommand(
        Guid? CommentId) : IRequest<Result>;
}
