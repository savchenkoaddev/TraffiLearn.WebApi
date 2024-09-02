using TraffiLearn.Domain.Aggregates.Tickets;
using TraffiLearn.Domain.Aggregates.Tickets.ValueObjects;

namespace TraffiLearn.Testing.Shared.Factories
{
    public static class TicketFixtureFactory
    {
        public static Ticket CreateTicket(int number = TicketNumber.MinValue)
        {
            return Ticket.Create(
                ticketId: new TicketId(Guid.NewGuid()),
                CreateNumber(number)).Value;
        }

        public static TicketNumber CreateNumber(int number = TicketNumber.MinValue)
        {
            return TicketNumber.Create(number).Value;
        }
    }
}
