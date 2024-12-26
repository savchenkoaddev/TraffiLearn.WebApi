using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.UseCases.Tickets.DTO;
using TraffiLearn.Domain.Tickets;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Tickets.Queries.GetById
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
                ticketId: new TicketId(request.TicketId),
                cancellationToken);

            if (ticket is null)
            {
                return Result.Failure<TicketResponse>(TicketErrors.NotFound);
            }

            return _ticketMapper.Map(ticket);
        }
    }
}
