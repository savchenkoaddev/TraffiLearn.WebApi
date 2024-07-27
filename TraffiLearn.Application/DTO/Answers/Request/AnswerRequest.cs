namespace TraffiLearn.Application.DTO.Answers.Request
{
    public sealed record AnswerRequest(
        string Text,
        bool IsCorrect);
}
