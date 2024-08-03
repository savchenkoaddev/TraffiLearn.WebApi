using FluentValidation;

namespace TraffiLearn.Application.Commands.Questions.Delete
{
    internal sealed class DeleteQuestionCommandValidator : AbstractValidator<DeleteQuestionCommand>
    {
        public DeleteQuestionCommandValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty();
        }
    }
}
