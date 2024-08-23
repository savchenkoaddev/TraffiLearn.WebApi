using TraffiLearn.Application.Questions.DTO;

namespace TraffiLearn.WebAPI.CommandWrappers.CreateQuestion
{
    public sealed record CreateQuestionRequest(
        string? Content,
        string? Explanation,
        int? QuestionNumber,
        List<Guid>? TopicIds,
        List<AnswerRequest>? Answers);
}
