using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Tickets.DTO;
using TraffiLearn.Domain.Aggregates.Tickets;

namespace TraffiLearn.Application.Tickets.Mappers
{
    internal sealed class TicketToTicketResponseMapper : Mapper<Ticket, TicketResponse>
    {
        public override TicketResponse Map(Ticket source)
        {
            return new TicketResponse(
                TicketId: source.Id.Value,
                TicketNumber: source.TicketNumber.Value);
        }
    }
}
