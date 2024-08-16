using FluentValidation;
using TraffiLearn.Domain.Aggregates.Comments.ValueObjects;

namespace TraffiLearn.Application.Commands.Comments.UpdateComment
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
