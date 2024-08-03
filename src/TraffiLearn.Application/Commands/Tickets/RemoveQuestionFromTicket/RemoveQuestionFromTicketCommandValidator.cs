using FluentValidation;

namespace TraffiLearn.Application.Commands.Tickets.RemoveQuestionFromTicket
{
    internal sealed class RemoveQuestionFromTicketCommandValidator 
        : AbstractValidator<RemoveQuestionFromTicketCommand>
    {
        public RemoveQuestionFromTicketCommandValidator()
        {
            RuleFor(x => x.TicketId)
                .NotEmpty();

            RuleFor(x => x.QuestionId)
                .NotEmpty();
        }
    }
}
