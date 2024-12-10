using FluentValidation;

namespace TraffiLearn.Application.UseCases.Users.Commands.MarkQuestion
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
