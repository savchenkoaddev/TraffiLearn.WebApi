using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Application.DTO.Questions.Request
{
    public sealed record QuestionCreateRequest(
        string? Content,
        string? Explanation,
        QuestionTitleDetails TitleDetails,
        IEnumerable<Guid?>? TopicsIds);
}
