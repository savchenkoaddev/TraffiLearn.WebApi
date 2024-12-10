using TraffiLearn.Application.UseCases.Questions.DTO;

namespace TraffiLearn.Application.UseCases.Tickets.DTO
{
    public sealed record TicketWithQuestionsResponse(
        Guid TicketId,
        int TicketNumber,
        IEnumerable<QuestionResponse> Questions);
}
