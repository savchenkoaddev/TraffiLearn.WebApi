using FluentValidation;

namespace TraffiLearn.Application.Users.Commands.RemoveQuestionLike
{
    internal sealed class RemoveQuestionLikeCommandValidator
        : AbstractValidator<RemoveQuestionLikeCommand>
    {
        public RemoveQuestionLikeCommandValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty();
        }
    }
}
