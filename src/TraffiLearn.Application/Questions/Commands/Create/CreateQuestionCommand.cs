using MediatR;
using Microsoft.AspNetCore.Http;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Questions.Commands.Create
{
    public sealed record CreateQuestionCommand(
        string? Content,
        string? Explanation,
        int? QuestionNumber,
        List<Guid>? TopicIds,
        List<AnswerRequest>? Answers,
        IFormFile? Image) : IRequest<Result<Guid>>;
}
