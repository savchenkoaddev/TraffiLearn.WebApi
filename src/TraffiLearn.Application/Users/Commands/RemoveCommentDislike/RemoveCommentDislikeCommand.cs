using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Users.Commands.RemoveCommentDislike
{
    public sealed record RemoveCommentDislikeCommand(
        Guid? CommentId) : IRequest<Result>;
}
