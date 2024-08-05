using MediatR;
using TraffiLearn.Application.DTO.Comments;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Comments.GetCommentReplies
{
    public sealed record GetCommentsRepliesQuery(
        Guid? CommentId) : IRequest<Result<IEnumerable<CommentResponse>>>;
}
