using MediatR;
using TraffiLearn.Application.Tickets.DTO;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Tickets.Queries.GetAll
{
    public sealed record GetAllTicketsQuery
        : IRequest<Result<IEnumerable<TicketResponse>>>;
}
