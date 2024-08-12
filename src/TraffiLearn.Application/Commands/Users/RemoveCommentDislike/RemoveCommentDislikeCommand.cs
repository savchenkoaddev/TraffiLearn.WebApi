using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Users.RemoveCommentDislike
{
    public sealed record RemoveCommentDislikeCommand(
        Guid? CommentId) : IRequest<Result>;
}
