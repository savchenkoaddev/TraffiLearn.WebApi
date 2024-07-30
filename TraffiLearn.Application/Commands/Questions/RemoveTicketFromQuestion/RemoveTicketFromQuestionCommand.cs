using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Questions.RemoveTicketFromQuestion
{
    public sealed record RemoveTicketFromQuestionCommand(
        Guid? TicketId,
        Guid? QuestionId) : IRequest<Result>;
}
