using FluentValidation;

namespace TraffiLearn.Application.Commands.Users.UnmarkQuestion
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
