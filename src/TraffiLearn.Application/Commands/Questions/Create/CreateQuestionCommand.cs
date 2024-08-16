using MediatR;
using Microsoft.AspNetCore.Http;
using TraffiLearn.Application.DTO.Answers;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Questions.Create
{
    public sealed record CreateQuestionCommand(
        string? Content,
        string? Explanation,
        int? QuestionNumber,
        List<Guid>? TopicIds,
        List<AnswerRequest?>? Answers,
        IFormFile? Image) : IRequest<Result>;
}
