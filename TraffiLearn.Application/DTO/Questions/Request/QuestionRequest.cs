using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Application.DTO.Questions.Request
{
    public sealed record QuestionRequest(
        string? Content,
        string? Explanation,
        QuestionTitleDetails TitleDetails);
}
