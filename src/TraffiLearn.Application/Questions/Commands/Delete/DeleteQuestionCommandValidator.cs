using FluentValidation;

namespace TraffiLearn.Application.Questions.Commands.Delete
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
