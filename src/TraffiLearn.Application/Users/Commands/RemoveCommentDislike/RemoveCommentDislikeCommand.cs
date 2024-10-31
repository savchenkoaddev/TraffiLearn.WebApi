﻿using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Users.Commands.RemoveCommentDislike
{
    public sealed record RemoveCommentDislikeCommand(
        Guid? CommentId) : IRequest<Result>;
}
