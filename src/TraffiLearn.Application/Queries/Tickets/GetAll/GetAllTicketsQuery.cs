using MediatR;
using TraffiLearn.Application.DTO.Tickets;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Tickets.GetAll
{
    public sealed record GetAllTicketsQuery
        : IRequest<Result<IEnumerable<TicketResponse>>>;
}
