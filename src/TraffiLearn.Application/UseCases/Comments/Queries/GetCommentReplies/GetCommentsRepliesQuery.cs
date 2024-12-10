using MediatR;
using TraffiLearn.Application.UseCases.Comments.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Comments.Queries.GetCommentReplies
{
    public sealed record GetCommentsRepliesQuery(
        Guid? CommentId) : IRequest<Result<IEnumerable<CommentResponse>>>;
}
