using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Application.DTO.Questions.Request
{
    public sealed record QuestionUpdateRequest(
        string? Content,
        string? Explanation,
        QuestionTitleDetails TitleDetails);
}
