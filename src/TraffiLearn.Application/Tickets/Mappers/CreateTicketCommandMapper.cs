using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Tickets.Commands.Create;
using TraffiLearn.Domain.Aggregates.Tickets;
using TraffiLearn.Domain.Aggregates.Tickets.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Tickets.Mappers
{
    internal sealed class CreateTicketCommandMapper
        : Mapper<CreateTicketCommand, Result<Ticket>>
    {
        public override Result<Ticket> Map(CreateTicketCommand source)
        {
            var numberCreateResult = TicketNumber.Create(source.TicketNumber.Value);

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
