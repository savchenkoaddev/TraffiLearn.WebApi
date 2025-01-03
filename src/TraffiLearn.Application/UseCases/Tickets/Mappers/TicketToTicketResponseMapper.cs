﻿using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.UseCases.Tickets.DTO;
using TraffiLearn.Domain.Tickets;

namespace TraffiLearn.Application.UseCases.Tickets.Mappers
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
