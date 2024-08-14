using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.ValueObjects.Tickets;

namespace TraffiLearn.DomainTests.Factories
{
    internal static class TicketFixtureFactory
    {
        public static Ticket CreateTicket()
        {
            return Ticket.Create(
                ticketId: new TicketId(Guid.NewGuid()),
                CreateNumber()).Value;
        }

        public static TicketNumber CreateNumber()
        {
            return TicketNumber.Create(TicketNumber.MinValue).Value;
        }
    }
}
