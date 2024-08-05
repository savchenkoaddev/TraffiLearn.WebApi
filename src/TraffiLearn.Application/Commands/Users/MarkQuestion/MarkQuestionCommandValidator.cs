using FluentValidation;

namespace TraffiLearn.Application.Commands.Users.MarkQuestion
{
    internal sealed class MarkQuestionCommandValidator
        : AbstractValidator<MarkQuestionCommand>
    {
        public MarkQuestionCommandValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty();
        }
    }
}
