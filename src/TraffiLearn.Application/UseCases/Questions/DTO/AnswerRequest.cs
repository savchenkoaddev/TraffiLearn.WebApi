namespace TraffiLearn.Application.UseCases.Questions.DTO
{
    public sealed record AnswerRequest(
        string Text,
        bool? IsCorrect);
}
