namespace TraffiLearn.Application.UseCases.Questions.DTO
{
    public sealed record QuestionResponse(
        Guid Id,
        string Content,
        int LikesCount,
        int DislikesCount,
        int QuestionNumber,
        List<AnswerResponse> Answers,
        string? Explanation,
        string? ImageUri);
}
