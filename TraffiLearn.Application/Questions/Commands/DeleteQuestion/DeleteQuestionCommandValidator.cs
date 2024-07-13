using FluentValidation;

namespace TraffiLearn.Application.Questions.Commands.DeleteQuestion
{
    public sealed class DeleteQuestionCommandValidator : AbstractValidator<DeleteQuestionCommand>
    {
        public DeleteQuestionCommandValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty();
        }
    }
}
