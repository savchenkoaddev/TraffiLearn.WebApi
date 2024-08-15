using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Application.DTO.Tickets
{
    public sealed record TicketWithQuestionsResponse(
        Guid TicketId,
        int TicketNumber,
        IEnumerable<Question> Questions);
}
