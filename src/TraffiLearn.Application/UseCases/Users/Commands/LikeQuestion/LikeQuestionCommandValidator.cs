using FluentValidation;

namespace TraffiLearn.Application.UseCases.Users.Commands.LikeQuestion
{
    internal sealed class LikeQuestionCommandValidator
        : AbstractValidator<LikeQuestionCommand>
    {
        public LikeQuestionCommandValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty();
        }
    }
}
