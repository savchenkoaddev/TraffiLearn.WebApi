using MediatR;
using TraffiLearn.Application.UseCases.Comments.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Queries.GetUserComments
{
    public sealed record GetUserCommentsQuery(
        Guid UserId) : IRequest<Result<IEnumerable<CommentResponse>>>;
}
