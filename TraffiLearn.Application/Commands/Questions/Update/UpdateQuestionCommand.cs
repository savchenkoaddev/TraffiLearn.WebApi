using MediatR;
using Microsoft.AspNetCore.Http;
using TraffiLearn.Application.DTO.Answers;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Questions.Update
{
    public sealed record UpdateQuestionCommand(
        Guid? QuestionId,
        string? Content,
        string? Explanation,
        int? QuestionNumber,
        List<AnswerRequest?>? Answers,
        List<Guid?>? TopicsIds,
        IFormFile? Image,
        bool? RemoveOldImageIfNewImageMissing = true) : IRequest<Result>;
}
