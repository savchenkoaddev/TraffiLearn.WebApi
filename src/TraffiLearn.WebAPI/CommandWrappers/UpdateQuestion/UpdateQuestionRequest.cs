namespace TraffiLearn.Application.Questions.DTO
{
    public sealed record UpdateQuestionRequest(
        Guid? QuestionId,
        string? Content,
        string? Explanation,
        int? QuestionNumber,
        List<AnswerRequest>? Answers,
        List<Guid>? TopicIds,
        bool? RemoveOldImageIfNewImageMissing = true);
}
