using FluentValidation;

namespace TraffiLearn.Application.Commands.Users.DislikeComment
{
    internal sealed class DislikeCommentCommandValidator
        : AbstractValidator<DislikeCommentCommand>
    {
        public DislikeCommentCommandValidator()
        {
            RuleFor(x => x.CommentId)
                .NotEmpty();
        }
    }
}
