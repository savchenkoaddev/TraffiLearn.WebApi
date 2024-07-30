using FluentValidation;

namespace TraffiLearn.Application.Commands.Tickets.Create
{
    internal sealed class CreateTicketCommandValidator 
        : AbstractValidator<CreateTicketCommand>
    {
        public CreateTicketCommandValidator()
        {
            RuleFor(x => x.TicketNumber)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
