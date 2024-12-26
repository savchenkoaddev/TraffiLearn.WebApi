using MediatR;
using Microsoft.AspNetCore.Http;
using TraffiLearn.Application.UseCases.Questions.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Questions.Commands.Create
{
    public sealed record CreateQuestionCommand(
        string Content,
        int QuestionNumber,
        List<Guid> TopicIds,
        List<AnswerRequest> Answers,
        string? Explanation,
        IFormFile? Image) : IRequest<Result<Guid>>;
}
