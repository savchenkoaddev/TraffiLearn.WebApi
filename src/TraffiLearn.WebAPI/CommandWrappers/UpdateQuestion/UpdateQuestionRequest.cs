using TraffiLearn.Application.UseCases.Questions.DTO;

namespace TraffiLearn.WebAPI.CommandWrappers.UpdateQuestion
{
    public sealed record UpdateQuestionRequest(
        Guid QuestionId,
        string Content,
        int QuestionNumber,
        List<AnswerRequest> Answers,
        List<Guid> TopicIds,
        string? Explanation,
        bool? RemoveOldImageIfNewMissing);
}
