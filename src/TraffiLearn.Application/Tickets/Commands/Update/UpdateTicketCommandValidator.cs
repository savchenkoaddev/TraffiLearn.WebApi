using FluentValidation;

namespace TraffiLearn.Application.Tickets.Commands.Update
{
    internal sealed class UpdateTicketCommandValidator
        : AbstractValidator<UpdateTicketCommand>
    {
        public UpdateTicketCommandValidator()
        {
            RuleFor(x => x.TicketId)
                .NotEmpty();

            RuleFor(x => x.TicketNumber)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(x => x.QuestionIds)
                .NotEmpty();

            RuleForEach(x => x.QuestionIds)
                .NotEmpty();
        }
    }
}
