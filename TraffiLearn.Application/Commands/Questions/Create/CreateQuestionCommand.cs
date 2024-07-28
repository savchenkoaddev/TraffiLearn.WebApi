using MediatR;
using Microsoft.AspNetCore.Http;
using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Application.Commands.Questions.Create
{
    public sealed record CreateQuestionCommand(
        string Content,
        string Explanation,
        int TicketNumber,
        int QuestionNumber,
        List<Guid> TopicsIds,
        List<Answer> Answers,
        IFormFile? Image) : IRequest;
}
