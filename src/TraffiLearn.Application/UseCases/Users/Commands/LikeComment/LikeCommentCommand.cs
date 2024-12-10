using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Commands.LikeComment
{
    public sealed record LikeCommentCommand(
        Guid? CommentId) : IRequest<Result>;
}
