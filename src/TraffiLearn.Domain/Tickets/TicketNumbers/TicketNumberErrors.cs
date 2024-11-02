using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.Tickets.TicketNumbers
{
    public static class TicketNumberErrors
    {
        public static Error TooSmall(int minValue) =>
            Error.Validation(
                code: "TicketNumber.TooSmall",
                description: $"Ticket number must be greater or equal to {minValue}");
    }
}
