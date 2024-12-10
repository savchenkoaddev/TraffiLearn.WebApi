using FluentValidation;

namespace TraffiLearn.Application.UseCases.Tickets.Commands.Delete
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
