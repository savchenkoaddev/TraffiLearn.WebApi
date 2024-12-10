namespace TraffiLearn.Application.UseCases.Questions.DTO
{
    public sealed record AnswerResponse(
        string Text,
        bool IsCorrect);
}
