using FluentValidation;

namespace TraffiLearn.Application.Users.Commands.UnmarkQuestion
{
    internal sealed class UnmarkQuestionCommandValidator
        : AbstractValidator<UnmarkQuestionCommand>
    {
        public UnmarkQuestionCommandValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty();
        }
    }
}
