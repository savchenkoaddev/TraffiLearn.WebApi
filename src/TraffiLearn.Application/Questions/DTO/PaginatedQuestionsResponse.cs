namespace TraffiLearn.Application.Questions.DTO
{
    public sealed record PaginatedQuestionsResponse(
        IEnumerable<QuestionResponse> Questions,
        int TotalPages);
}
