using FluentValidation;

namespace TraffiLearn.Application.UseCases.Questions.Commands.Delete
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
