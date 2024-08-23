namespace TraffiLearn.Application.Questions.DTO
{
    public sealed record AnswerResponse(
        string Text,
        bool IsCorrect);
}
