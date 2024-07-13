using FluentValidation;

namespace TraffiLearn.Application.Questions.Commands.CreateQuestion
{
    public sealed class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
    {
        public CreateQuestionCommandValidator()
        {
            RuleFor(x => x.RequestObject)
                .NotNull();

            RuleFor(x => x.RequestObject.Explanation)
                .NotEmpty()
                .When(x => x.RequestObject is not null);

            RuleFor(x => x.RequestObject.Content)
                .NotEmpty()
                .MaximumLength(3000)
                .When(x => x.RequestObject is not null);

            RuleFor(x => x.RequestObject.TitleDetails.QuestionNumber)
                .NotEmpty()
                .When(x => x.RequestObject.TitleDetails.TicketNumber is not null)
                .When(x => x.RequestObject is not null);

            RuleFor(x => x.RequestObject.TitleDetails.TicketNumber)
               .NotEmpty()
               .When(x => x.RequestObject.TitleDetails.QuestionNumber is not null)
               .When(x => x.RequestObject is not null);

            RuleFor(x => x.RequestObject.TopicsIds)
                .NotNull()
                .When(x => x.RequestObject is not null);

            RuleForEach(x => x.RequestObject.TopicsIds)
                .NotEmpty()
                .When(x => x.RequestObject.TopicsIds is not null)
                .When(x => x.RequestObject is not null);
        }
    }
}
