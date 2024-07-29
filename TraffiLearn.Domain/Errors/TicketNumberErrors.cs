using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.Errors
{
    public static class TicketNumberErrors
    {
        public static Error TooSmall(int minValue) =>
            Error.Validation("TicketNumber.TooSmall", $"Ticket number must be greater or equal to {minValue}");
    }
}
