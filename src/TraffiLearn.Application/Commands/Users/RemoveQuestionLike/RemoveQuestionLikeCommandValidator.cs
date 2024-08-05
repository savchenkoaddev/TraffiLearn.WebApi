using FluentValidation;

namespace TraffiLearn.Application.Commands.Users.RemoveQuestionLike
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
