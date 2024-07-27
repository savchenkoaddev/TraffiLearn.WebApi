using TraffiLearn.Application.DTO.Answers.Request;
using TraffiLearn.Application.DTO.QuestionTitleDetails.Request;

namespace TraffiLearn.Application.DTO.Questions.Request
{
    public sealed record QuestionUpdateRequest(
        string Content,
        string Explanation,
        QuestionTitleDetailsRequest TitleDetails,
        IEnumerable<AnswerRequest> Answers,
        IEnumerable<Guid> TopicsIds,
        bool RemoveOldImageIfNewImageMissing = true);
}
