namespace TraffiLearn.Application.Tickets.DTO
{
    public sealed record TicketResponse(
        Guid TicketId,
        int TicketNumber);
}
