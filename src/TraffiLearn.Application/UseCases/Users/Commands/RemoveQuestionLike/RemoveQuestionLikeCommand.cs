﻿using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Commands.RemoveQuestionLike
{
    public sealed record RemoveQuestionLikeCommand(
        Guid QuestionId) : IRequest<Result>;
}
