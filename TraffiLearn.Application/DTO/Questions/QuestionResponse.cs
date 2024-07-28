using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Application.DTO.Questions
{
    public sealed record QuestionResponse(
        Guid Id,
        QuestionContent Content,
        QuestionExplanation Explanation,
        ImageUri? ImageUri,
        int LikesCount,
        int DislikesCount,
        TicketNumber TicketNumber,
        QuestionNumber QuestionNumber,
        List<Answer> Answers);
}
