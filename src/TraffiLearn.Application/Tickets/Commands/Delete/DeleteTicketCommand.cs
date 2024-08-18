using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Tickets.Commands.Delete
{
    public sealed record DeleteTicketCommand(
        Guid? TicketId) : IRequest<Result>;
}
