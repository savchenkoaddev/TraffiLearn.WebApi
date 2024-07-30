using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Tickets.Update
{
    public sealed record UpdateTicketCommand(
        Guid? TicketId,
        int? TicketNumber,
        List<Guid?>? QuestionsIds) : IRequest<Result>;
}
