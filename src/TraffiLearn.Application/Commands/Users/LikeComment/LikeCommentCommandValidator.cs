using FluentValidation;

namespace TraffiLearn.Application.Commands.Users.LikeComment
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
