namespace TraffiLearn.Application.UseCases.Questions.DTO
{
    public sealed record PaginatedQuestionsResponse(
        IEnumerable<QuestionResponse> Questions,
        int TotalPages);
}
