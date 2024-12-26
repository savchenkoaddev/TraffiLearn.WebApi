using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.UseCases.Tickets.Commands.Create;
using TraffiLearn.Domain.Tickets;
using TraffiLearn.Domain.Tickets.TicketNumbers;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Tickets.Mappers
{
    internal sealed class CreateTicketCommandMapper
        : Mapper<CreateTicketCommand, Result<Ticket>>
    {
        public override Result<Ticket> Map(CreateTicketCommand source)
        {
            var numberCreateResult = TicketNumber.Create(source.TicketNumber);

            if (numberCreateResult.IsFailure)
            {
                return Result.Failure<Ticket>(numberCreateResult.Error);
            }

            var ticketId = Guid.NewGuid();

            return Ticket.Create(
                new TicketId(ticketId),
                numberCreateResult.Value);
        }
    }
}
