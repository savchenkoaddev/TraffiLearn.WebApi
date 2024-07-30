namespace TraffiLearn.Application.DTO.Answers
{
    public sealed record AnswerRequest(
        string? Text,
        bool? IsCorrect);
}
