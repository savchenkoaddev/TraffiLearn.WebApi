using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Tickets;

namespace TraffiLearn.Application.Commands.Tickets.Create
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

            var ticketId = new TicketId(Guid.NewGuid());

            return Ticket.Create(
                ticketId,
                numberCreateResult.Value);
        }
    }
}
