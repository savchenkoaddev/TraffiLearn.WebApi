using MediatR;
using TraffiLearn.Application.Comments.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Users.Queries.GetUserComments
{
    public sealed record GetUserCommentsQuery(
        Guid? UserId) : IRequest<Result<IEnumerable<CommentResponse>>>;
}
