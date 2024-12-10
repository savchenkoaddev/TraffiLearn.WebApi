using FluentValidation;

namespace TraffiLearn.Application.UseCases.Users.Commands.DislikeQuestion
{
    internal sealed class DislikeQuestionCommandValidator
        : AbstractValidator<DislikeQuestionCommand>
    {
        public DislikeQuestionCommandValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty();
        }
    }
}
