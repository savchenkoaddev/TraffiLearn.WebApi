namespace TraffiLearn.Application.Questions.DTO
{
    public sealed record AnswerRequest(
        string? Text,
        bool? IsCorrect);
}
