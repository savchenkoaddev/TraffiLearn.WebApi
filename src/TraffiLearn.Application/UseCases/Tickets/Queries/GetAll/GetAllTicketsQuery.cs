using MediatR;
using TraffiLearn.Application.UseCases.Tickets.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Tickets.Queries.GetAll
{
    public sealed record GetAllTicketsQuery
        : IRequest<Result<IEnumerable<TicketResponse>>>;
}
