namespace TraffiLearn.Application.UseCases.Tickets.DTO
{
    public sealed record TicketResponse(
        Guid TicketId,
        int TicketNumber);
}
