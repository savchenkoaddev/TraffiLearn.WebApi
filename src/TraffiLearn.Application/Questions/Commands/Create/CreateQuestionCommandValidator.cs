using FluentValidation;

namespace TraffiLearn.Application.Questions.Commands.Create
{
    internal sealed class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
    {
        public CreateQuestionCommandValidator()
        {
            RuleFor(x => x.Explanation)
                .NotEmpty();

            RuleFor(x => x.Content)
                .NotEmpty()
                .MaximumLength(3000);

            RuleFor(x => x.QuestionNumber)
                .GreaterThan(0)
                .NotEmpty();

            RuleFor(x => x.TopicsIds)
                .NotEmpty();

            RuleForEach(x => x.TopicsIds)
                .NotEmpty()
                .When(x => x.TopicsIds is not null);

            RuleFor(x => x.Answers)
                .NotEmpty();

            RuleForEach(x => x.Answers)
                .NotEmpty()
                .ChildRules(x =>
                {
                    x.RuleFor(x => x.Text)
                        .NotEmpty()
                        .MaximumLength(300);

                    x.RuleFor(x => x.IsCorrect)
                        .NotEmpty();
                })
                .When(x => x.Answers is not null);
        }
    }
}
