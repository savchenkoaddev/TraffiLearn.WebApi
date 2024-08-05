using FluentValidation;

namespace TraffiLearn.Application.Commands.Users.LikeQuestion
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
