﻿using FluentValidation;

namespace TraffiLearn.Application.UseCases.Users.Commands.LikeComment
{
    internal sealed class LikeCommentCommandValidator
        : AbstractValidator<LikeCommentCommand>
    {
        public LikeCommentCommandValidator()
        {
            RuleFor(x => x.CommentId)
                .NotEmpty();
        }
    }
}
