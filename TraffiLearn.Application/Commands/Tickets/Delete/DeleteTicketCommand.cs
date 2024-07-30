using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Tickets.Delete
{
    public sealed record DeleteTicketCommand(
        Guid? TicketId) : IRequest<Result>;
}
