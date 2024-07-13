using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Application.DTO.Questions.Response
{
    public sealed record QuestionResponse(
        Guid Id,
        string Content,
        string Explanation,
        int LikesCount,
        int DislikesCount,
        QuestionTitleDetails TitleDetails);
}
