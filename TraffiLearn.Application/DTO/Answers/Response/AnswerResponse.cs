namespace TraffiLearn.Application.DTO.Answers.Response
{
    public sealed record AnswerResponse(
        string Text,
        bool IsCorrect);
}
