namespace TraffiLearn.Application.DTO.Tickets
{
    public sealed record TicketResponse(
        Guid TicketId,
        int TicketNumber);
}
