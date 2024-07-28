using FluentValidation;

namespace TraffiLearn.Application.Commands.Questions.Create
{
    public sealed class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
    {
        public CreateQuestionCommandValidator()
        {
            RuleFor(x => x.Explanation)
                .NotEmpty();

            RuleFor(x => x.Content)
                .NotEmpty()
                .MaximumLength(3000);

            RuleFor(x => x.QuestionNumber)
                .GreaterThan(0);

            RuleFor(x => x.TicketNumber)
                .GreaterThan(0);

            RuleFor(x => x.TopicsIds)
                .NotEmpty();

            RuleForEach(x => x.TopicsIds)
                .NotEmpty();

            RuleFor(x => x.Answers)
                .NotEmpty();
        }
    }
}
