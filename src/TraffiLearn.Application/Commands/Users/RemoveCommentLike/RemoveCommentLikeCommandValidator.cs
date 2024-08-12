using FluentValidation;

namespace TraffiLearn.Application.Commands.Users.RemoveCommentLike
{
    internal sealed class RemoveCommentLikeCommandValidator
        : AbstractValidator<RemoveCommentLikeCommand>
    {
        public RemoveCommentLikeCommandValidator()
        {
            RuleFor(x => x.CommentId)
                .NotEmpty();
        }
    }
}
