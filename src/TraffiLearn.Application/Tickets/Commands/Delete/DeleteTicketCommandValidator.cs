using FluentValidation;

namespace TraffiLearn.Application.Tickets.Commands.Delete
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
