using FluentValidation;

namespace TraffiLearn.Application.Users.Commands.RemoveQuestionDislike
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
