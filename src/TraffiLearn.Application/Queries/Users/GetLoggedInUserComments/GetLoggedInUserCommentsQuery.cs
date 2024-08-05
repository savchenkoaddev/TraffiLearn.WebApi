using MediatR;
using TraffiLearn.Application.DTO.Comments;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Users.GetLoggedInUserComments
{
    public sealed record GetLoggedInUserCommentsQuery
        : IRequest<Result<IEnumerable<CommentResponse>>>;
}
