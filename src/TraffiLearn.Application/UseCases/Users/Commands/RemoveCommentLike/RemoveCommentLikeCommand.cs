using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Commands.RemoveCommentLike
{
    public sealed record RemoveCommentLikeCommand(
        Guid CommentId) : IRequest<Result>;
}
