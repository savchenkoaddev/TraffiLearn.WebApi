using Microsoft.AspNetCore.Http;
using TraffiLearn.Application.DTO.Answers.Request;
using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Application.DTO.Questions.Request
{
    public sealed record QuestionUpdateRequest(
        string? Content,
        string? Explanation,
        QuestionTitleDetails? TitleDetails,
        IEnumerable<AnswerRequest?>? Answers,
        IFormFile? Image);
}
