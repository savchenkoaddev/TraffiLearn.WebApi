using FluentValidation;

namespace TraffiLearn.Application.UseCases.Users.Commands.DislikeComment
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
