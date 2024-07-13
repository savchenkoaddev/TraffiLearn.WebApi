using FluentValidation;

namespace TraffiLearn.Application.Questions.Commands.UpdateQuestion
{
    public sealed class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
    {
        public UpdateQuestionCommandValidator()
        {
            RuleFor(x => x.RequestObject)
                .NotNull();

            RuleFor(x => x.QuestionId)
                .NotEmpty();

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
        }
    }
}
