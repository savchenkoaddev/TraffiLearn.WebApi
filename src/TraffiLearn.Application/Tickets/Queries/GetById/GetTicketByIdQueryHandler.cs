using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Tickets.DTO;
using TraffiLearn.Domain.Aggregates.Tickets;
using TraffiLearn.Domain.Aggregates.Tickets.Errors;
using TraffiLearn.Domain.Aggregates.Tickets.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Tickets.Queries.GetById
{
    internal sealed class GetTicketByIdQueryHandler
        : IRequestHandler<GetTicketByIdQuery, Result<TicketResponse>>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly Mapper<Ticket, TicketResponse> _ticketMapper;

        public GetTicketByIdQueryHandler(
            ITicketRepository ticketRepository,
            Mapper<Ticket, TicketResponse> ticketMapper)
        {
            _ticketRepository = ticketRepository;
            _ticketMapper = ticketMapper;
        }

        public async Task<Result<TicketResponse>> Handle(
            GetTicketByIdQuery request,
            CancellationToken cancellationToken)
        {
            var ticket = await _ticketRepository.GetByIdAsync(
                ticketId: new TicketId(request.TicketId.Value),
                cancellationToken);

            if (ticket is null)
            {
                return Result.Failure<TicketResponse>(TicketErrors.NotFound);
            }

            return _ticketMapper.Map(ticket);
        }
    }
}
