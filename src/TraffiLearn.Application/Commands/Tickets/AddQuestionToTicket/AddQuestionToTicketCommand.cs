using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Tickets.AddQuestionToTicket
{
    public sealed record AddQuestionToTicketCommand(
        Guid? QuestionId,
        Guid? TicketId) : IRequest<Result>;
}
