using FluentValidation;

namespace TraffiLearn.Application.Commands.Users.RemoveQuestionDislike
{
    internal sealed class RemoveQuestionDislikeCommandValidator
        : AbstractValidator<RemoveQuestionDislikeCommand>
    {
        public RemoveQuestionDislikeCommandValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty();
        }
    }
}
