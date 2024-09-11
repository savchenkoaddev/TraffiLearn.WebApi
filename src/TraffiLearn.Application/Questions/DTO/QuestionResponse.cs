namespace TraffiLearn.Application.Questions.DTO
{
    public sealed record QuestionResponse(
        Guid Id,
        string Content,
        string? Explanation,
        string? ImageUri,
        int LikesCount,
        int DislikesCount,
        int QuestionNumber,
        List<AnswerResponse> Answers);
}
