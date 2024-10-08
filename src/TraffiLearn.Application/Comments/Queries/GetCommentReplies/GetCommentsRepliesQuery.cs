﻿using MediatR;
using TraffiLearn.Application.Comments.DTO;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Comments.Queries.GetCommentReplies
{
    public sealed record GetCommentsRepliesQuery(
        Guid? CommentId) : IRequest<Result<IEnumerable<CommentResponse>>>;
}
