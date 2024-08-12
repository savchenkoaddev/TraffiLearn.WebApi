using FluentValidation;

namespace TraffiLearn.Application.Commands.Tickets.Update
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

            RuleFor(x => x.QuestionsIds)
                .NotEmpty();

            RuleForEach(x => x.QuestionsIds)
                .NotEmpty();
        }
    }
}
