using MediatR;
using Microsoft.AspNetCore.Http;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Domain.Shared;

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
        bool? RemoveOldImageIfNewImageMissing = true) : IRequest<Result>;
}
