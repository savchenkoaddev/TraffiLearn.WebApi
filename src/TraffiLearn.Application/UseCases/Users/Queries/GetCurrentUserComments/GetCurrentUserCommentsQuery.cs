using MediatR;
using TraffiLearn.Application.UseCases.Comments.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Queries.GetCurrentUserComments
{
    public sealed record GetCurrentUserCommentsQuery
        : IRequest<Result<IEnumerable<CommentResponse>>>;
}
