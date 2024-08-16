using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Tickets;
using TraffiLearn.Domain.Aggregates.Tickets;

namespace TraffiLearn.Application.Mappers.Tickets
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
