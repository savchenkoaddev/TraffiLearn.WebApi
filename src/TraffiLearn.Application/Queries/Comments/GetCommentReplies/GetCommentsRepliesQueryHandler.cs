using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Comments;
using TraffiLearn.Domain.Aggregates.Comments;
using TraffiLearn.Domain.Aggregates.Comments.Errors;
using TraffiLearn.Domain.Aggregates.Comments.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Comments.GetCommentReplies
{
    internal sealed class GetCommentsRepliesQueryHandler
        : IRequestHandler<GetCommentsRepliesQuery, Result<IEnumerable<CommentResponse>>>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly Mapper<Comment, CommentResponse> _commentMapper;

        public GetCommentsRepliesQueryHandler(
            ICommentRepository commentRepository,
            Mapper<Comment, CommentResponse> commentMapper)
        {
            _commentRepository = commentRepository;
            _commentMapper = commentMapper;
        }

        public async Task<Result<IEnumerable<CommentResponse>>> Handle(
            GetCommentsRepliesQuery request,
            CancellationToken cancellationToken)
        {
            var comment = await _commentRepository
                .GetByIdWithRepliesWithUsersTwoLevelsDeepAsync(
                    commentId: new CommentId(request.CommentId.Value),
                    cancellationToken);

            if (comment is null)
            {
                return Result.Failure<IEnumerable<CommentResponse>>(
                    CommentErrors.NotFound);
            }


            return Result.Success(_commentMapper.Map(comment.Replies));
        }
    }
}
