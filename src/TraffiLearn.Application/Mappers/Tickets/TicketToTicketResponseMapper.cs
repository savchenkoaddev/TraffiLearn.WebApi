using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Tickets;
using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Application.Mappers.Tickets
{
    internal sealed class TicketToTicketResponseMapper : Mapper<Ticket, TicketResponse>
    {
        public override TicketResponse Map(Ticket source)
        {
            return new TicketResponse(
                Id: source.Id,
                TicketNumber: source.TicketNumber.Value);
        }
    }
}
