using FluentValidation;

namespace TraffiLearn.Application.Commands.Users.RemoveCommentDislike
{
    internal sealed class RemoveCommentDislikeCommandValidator
        : AbstractValidator<RemoveCommentDislikeCommand>
    {
        public RemoveCommentDislikeCommandValidator()
        {
            RuleFor(x => x.CommentId)
                .NotEmpty();
        }
    }
}
