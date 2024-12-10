using FluentValidation;

namespace TraffiLearn.Application.UseCases.Users.Commands.RemoveCommentLike
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
