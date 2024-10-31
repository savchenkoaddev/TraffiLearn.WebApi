using MediatR;
using TraffiLearn.Application.Comments.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Users.Queries.GetLoggedInUserComments
{
    public sealed record GetLoggedInUserCommentsQuery
        : IRequest<Result<IEnumerable<CommentResponse>>>;
}
