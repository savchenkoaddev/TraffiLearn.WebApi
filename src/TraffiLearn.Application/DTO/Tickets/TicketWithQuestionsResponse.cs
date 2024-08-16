using TraffiLearn.Application.DTO.Questions;

namespace TraffiLearn.Application.DTO.Tickets
{
    public sealed record TicketWithQuestionsResponse(
        Guid TicketId,
        int TicketNumber,
        IEnumerable<QuestionResponse> Questions);
}
