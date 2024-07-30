namespace TraffiLearn.Application.DTO.Tickets
{
    public sealed record TicketResponse(
        Guid Id,
        string TicketNumber);
}
