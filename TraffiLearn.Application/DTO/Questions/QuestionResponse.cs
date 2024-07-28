using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Application.DTO.Questions
{
    public sealed record QuestionResponse(
        Guid Id,
        string Content,
        string Explanation,
        string? ImageUri,
        int LikesCount,
        int DislikesCount,
        int TicketNumber,
        int QuestionNumber,
        List<Answer> Answers);
}
