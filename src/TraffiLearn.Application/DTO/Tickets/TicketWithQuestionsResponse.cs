using TraffiLearn.Domain.Aggregates.Questions;

namespace TraffiLearn.Application.DTO.Tickets
{
    public sealed record TicketWithQuestionsResponse(
        Guid TicketId,
        int TicketNumber,
        IEnumerable<Question> Questions);
}
