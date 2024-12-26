using MediatR;
using Microsoft.AspNetCore.Http;
using TraffiLearn.Application.UseCases.Questions.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Questions.Commands.Update
{
    public sealed record UpdateQuestionCommand(
        Guid QuestionId,
        string Content,
        int QuestionNumber,
        List<AnswerRequest> Answers,
        List<Guid> TopicIds,
        string? Explanation,
        IFormFile? Image,
        bool RemoveOldImageIfNewMissing = true) : IRequest<Result>;
}
