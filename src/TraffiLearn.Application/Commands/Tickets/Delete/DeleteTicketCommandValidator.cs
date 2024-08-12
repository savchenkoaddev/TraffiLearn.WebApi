using FluentValidation;

namespace TraffiLearn.Application.Commands.Tickets.Delete
{
    internal sealed class DeleteTicketCommandValidator
        : AbstractValidator<DeleteTicketCommand>
    {
        public DeleteTicketCommandValidator()
        {
            RuleFor(x => x.TicketId)
                .NotEmpty();
        }
    }
}
