using FluentValidation;

namespace TraffiLearn.Application.Questions.Commands.CreateQuestion
{
    public sealed class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
    {
        public CreateQuestionCommandValidator()
        {
            RuleFor(x => x.RequestObject)
                .NotEmpty();

            RuleFor(x => x.RequestObject.Explanation)
                .NotEmpty()
                .When(x => x.RequestObject is not null);

            RuleFor(x => x.RequestObject.Content)
                .NotEmpty()
                .MaximumLength(3000)
                .When(x => x.RequestObject is not null);

            RuleFor(x => x.RequestObject.TitleDetails)
                .NotEmpty();

            RuleFor(x => x.RequestObject.TitleDetails.QuestionNumber)
                .NotEmpty()
                .GreaterThan(0)
                .When(x => x.RequestObject.TitleDetails.TicketNumber is not null)
                .When(x => x.RequestObject is not null);

            RuleFor(x => x.RequestObject.TitleDetails.TicketNumber)
               .NotEmpty()
               .GreaterThan(0)
               .When(x => x.RequestObject.TitleDetails.QuestionNumber is not null)
               .When(x => x.RequestObject is not null);

            RuleFor(x => x.RequestObject.TopicsIds)
                .NotEmpty()
                .When(x => x.RequestObject is not null);

            RuleFor(x => x.RequestObject.Answers)
                .NotEmpty();

            RuleForEach(x => x.RequestObject.Answers)
                .NotEmpty()
                .ChildRules(rules =>
                {
                    rules.RuleFor(x => x.IsCorrect)
                        .NotEmpty();

                    rules.RuleFor(x => x.Text)
                        .NotEmpty()
                        .MaximumLength(300);
                })
                .When(x => x.RequestObject.Answers is not null);

            RuleForEach(x => x.RequestObject.TopicsIds)
                .NotEmpty()
                .When(x => x.RequestObject.TopicsIds is not null)
                .When(x => x.RequestObject is not null);
        }
    }
}
