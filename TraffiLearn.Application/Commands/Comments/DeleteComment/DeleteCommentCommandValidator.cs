using FluentValidation;

namespace TraffiLearn.Application.Commands.Comments.DeleteComment
{
    internal sealed class DeleteCommentCommandValidator
        : AbstractValidator<DeleteCommentCommand>
    {
        public DeleteCommentCommandValidator()
        {
            RuleFor(x => x.CommentId)
                .NotEmpty();
        }
    }
}
