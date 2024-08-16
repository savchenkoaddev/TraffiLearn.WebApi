using FluentValidation;
using TraffiLearn.Domain.Aggregates.Comments.ValueObjects;

namespace TraffiLearn.Application.Commands.Questions.AddComment
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
