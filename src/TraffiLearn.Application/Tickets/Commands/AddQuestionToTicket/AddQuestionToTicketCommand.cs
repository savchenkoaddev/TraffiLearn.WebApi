using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Tickets.Commands.AddQuestionToTicket
{
    public sealed record AddQuestionToTicketCommand(
        Guid? QuestionId,
        Guid? TicketId) : IRequest<Result>;
}
