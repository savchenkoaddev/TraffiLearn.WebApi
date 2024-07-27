using TraffiLearn.Application.DTO.Answers.Response;
using TraffiLearn.Application.DTO.QuestionTitleDetails.Response;
using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Application.DTO.Questions.Response
{
    public sealed record QuestionResponse(
        Guid Id,
        string Content,
        string Explanation,
        string? ImageUri,
        int LikesCount,
        int DislikesCount,
        QuestionTitleDetailsResponse QuestionTitleDetails,
        List<AnswerResponse> Answers);
}
