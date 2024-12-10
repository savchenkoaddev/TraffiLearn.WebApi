using MediatR;
using TraffiLearn.Application.UseCases.Tickets.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Tickets.Queries.GetById
{
    public sealed record GetTicketByIdQuery(
        Guid? TicketId) : IRequest<Result<TicketResponse>>;
}
