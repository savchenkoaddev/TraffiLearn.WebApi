using FluentValidation;

namespace TraffiLearn.Application.Commands.Questions.AddTicketToQuestion
{
    internal sealed class AddTicketToQuestionCommandValidator
        : AbstractValidator<AddTicketToQuestionCommand>
    {
        public AddTicketToQuestionCommandValidator()
        {
            RuleFor(x => x.TicketId)
                .NotEmpty();

            RuleFor(x => x.QuestionId)
                .NotEmpty();
        }
    }
}
