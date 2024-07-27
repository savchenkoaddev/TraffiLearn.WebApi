using TraffiLearn.Application.DTO.Answers.Request;
using TraffiLearn.Application.DTO.QuestionTitleDetails.Request;

namespace TraffiLearn.Application.DTO.Questions.Request
{
    public sealed record QuestionCreateRequest(
        string Content,
        string Explanation,
        QuestionTitleDetailsRequest TitleDetails,
        IEnumerable<Guid> TopicsIds,
        IEnumerable<AnswerRequest> Answers);
}
