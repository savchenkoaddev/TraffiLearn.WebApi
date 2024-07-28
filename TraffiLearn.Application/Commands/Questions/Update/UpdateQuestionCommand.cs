using MediatR;
using Microsoft.AspNetCore.Http;
using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Application.Commands.Questions.Update
{
    public sealed record UpdateQuestionCommand(
        Guid QuestionId,
        string Content,
        string Explanation,
        int TicketNumber,
        int QuestionNumber,
        List<Answer> Answers,
        List<Guid> TopicsIds,
        IFormFile? Image,
        bool RemoveOldImageIfNewImageMissing = true) : IRequest;
}
