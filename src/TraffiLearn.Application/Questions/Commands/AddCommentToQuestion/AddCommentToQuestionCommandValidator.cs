using FluentValidation;
using TraffiLearn.Domain.Aggregates.Comments.ValueObjects;

namespace TraffiLearn.Application.Questions.Commands.AddCommentToQuestion
{
    internal sealed class AddCommentToQuestionCommandValidator
        : AbstractValidator<AddCommentToQuestionCommand>
    {
        public AddCommentToQuestionCommandValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .MaximumLength(CommentContent.MaxLength);

            RuleFor(x => x.QuestionId)
                .NotEmpty();
        }
    }
}
