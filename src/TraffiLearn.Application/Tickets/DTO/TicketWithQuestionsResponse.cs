using TraffiLearn.Application.Questions.DTO;

namespace TraffiLearn.Application.Tickets.DTO
{
    public sealed record TicketWithQuestionsResponse(
        Guid TicketId,
        int TicketNumber,
        IEnumerable<QuestionResponse> Questions);
}
