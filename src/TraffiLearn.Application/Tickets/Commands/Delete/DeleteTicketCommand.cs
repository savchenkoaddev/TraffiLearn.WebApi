using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Tickets.Commands.Delete
{
    public sealed record DeleteTicketCommand(
        Guid? TicketId) : IRequest<Result>;
}
