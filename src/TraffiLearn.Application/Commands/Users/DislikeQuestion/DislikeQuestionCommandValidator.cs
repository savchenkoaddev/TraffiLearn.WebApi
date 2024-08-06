using FluentValidation;

namespace TraffiLearn.Application.Commands.Users.DislikeQuestion
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
