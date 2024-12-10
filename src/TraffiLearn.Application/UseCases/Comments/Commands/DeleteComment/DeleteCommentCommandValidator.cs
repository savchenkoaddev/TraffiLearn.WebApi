using FluentValidation;

namespace TraffiLearn.Application.UseCases.Comments.Commands.DeleteComment
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
