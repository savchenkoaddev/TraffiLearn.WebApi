using TraffiLearn.Application.Questions.DTO;

namespace TraffiLearn.WebAPI.CommandWrappers.UpdateQuestion
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
