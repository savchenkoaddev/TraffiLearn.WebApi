using FluentValidation;

namespace TraffiLearn.Application.Users.Commands.RemoveCommentDislike
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
