using MediatR;
using TraffiLearn.Application.Tickets.DTO;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Tickets.Queries.GetById
{
    public sealed record GetTicketByIdQuery(
        Guid? TicketId) : IRequest<Result<TicketResponse>>;
}
