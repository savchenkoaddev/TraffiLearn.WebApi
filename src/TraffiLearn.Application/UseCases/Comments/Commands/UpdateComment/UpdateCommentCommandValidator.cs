using FluentValidation;
using TraffiLearn.Domain.Comments.CommentContents;

namespace TraffiLearn.Application.UseCases.Comments.Commands.UpdateComment
{
    internal sealed class UpdateCommentCommandValidator
        : AbstractValidator<UpdateCommentCommand>
    {
        public UpdateCommentCommandValidator()
        {
            RuleFor(x => x.CommentId)
                .NotEmpty();

            RuleFor(x => x.Content)
                .NotEmpty()
                .MaximumLength(CommentContent.MaxLength);
        }
    }
}
