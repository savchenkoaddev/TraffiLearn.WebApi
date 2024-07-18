using TraffiLearn.Application.DTO.Answers.Response;
using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Application.DTO.Questions.Response
{
    public sealed record QuestionResponse(
        Guid Id,
        string Content,
        string Explanation,
        string? ImageName,
        int LikesCount,
        int DislikesCount,
        QuestionTitleDetails TitleDetails,
        IEnumerable<AnswerResponse> Answers);
}
