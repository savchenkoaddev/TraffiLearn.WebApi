using FluentValidation;
using TraffiLearn.Domain.ValueObjects.Comments;

namespace TraffiLearn.Application.Commands.Comments.AddComment
{
    internal sealed class AddCommentCommandValidator
        : AbstractValidator<AddCommentCommand>
    {
        public AddCommentCommandValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .MaximumLength(CommentContent.MaxLength);

            RuleFor(x => x.QuestionId)
                .NotEmpty();
        }
    }
}
