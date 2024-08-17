using FluentValidation;

namespace TraffiLearn.Application.Users.Commands.RemoveCommentLike
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
