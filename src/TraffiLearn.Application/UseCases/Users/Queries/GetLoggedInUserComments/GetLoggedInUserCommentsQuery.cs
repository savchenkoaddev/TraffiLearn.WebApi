using MediatR;
using TraffiLearn.Application.UseCases.Comments.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Queries.GetLoggedInUserComments
{
    public sealed record GetLoggedInUserCommentsQuery
        : IRequest<Result<IEnumerable<CommentResponse>>>;
}
