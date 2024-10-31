﻿using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Comments.DTO;
using TraffiLearn.Domain.Aggregates.Comments;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Comments.Queries.GetCommentReplies
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
            var commentId = new CommentId(request.CommentId.Value);

            var commentExists = await _commentRepository.ExistsAsync(
                commentId,
                cancellationToken);

            if (!commentExists)
            {
                return Result.Failure<IEnumerable<CommentResponse>>(CommentErrors.NotFound);
            }

            var replies = await _commentRepository
                .GetRepliesWithNextRepliesByIdAsync(
                    commentId,
                    cancellationToken);

            return Result.Success(_commentMapper.Map(replies));
        }
    }
}
