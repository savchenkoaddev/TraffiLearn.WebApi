using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Commands.DislikeComment
{
    public sealed record DislikeCommentCommand(
        Guid CommentId) : IRequest<Result>;
}
