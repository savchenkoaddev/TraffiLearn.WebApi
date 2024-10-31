using MediatR;
using Microsoft.AspNetCore.Http;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Questions.Commands.Update
{
    public sealed record UpdateQuestionCommand(
        Guid? QuestionId,
        string? Content,
        string? Explanation,
        int? QuestionNumber,
        List<AnswerRequest>? Answers,
        List<Guid>? TopicIds,
        IFormFile? Image,
        bool RemoveOldImageIfNewMissing = true) : IRequest<Result>;
}
