﻿using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Comments.Commands.UpdateComment
{
    public sealed record UpdateCommentCommand(
        Guid? CommentId,
        string? Content) : IRequest<Result>;
}
