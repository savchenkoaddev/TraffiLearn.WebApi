using MediatR;
using TraffiLearn.Application.DTO.Comments;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Users.GetUserComments
{
    public sealed record GetUserCommentsQuery(
        Guid? UserId) : IRequest<Result<IEnumerable<CommentResponse>>>;
}
