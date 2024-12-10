using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.UseCases.Tickets.DTO;
using TraffiLearn.Domain.Tickets;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Tickets.Queries.GetAll
{
    internal sealed class GetAllTicketsQueryHandler
        : IRequestHandler<GetAllTicketsQuery, Result<IEnumerable<TicketResponse>>>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly Mapper<Ticket, TicketResponse> _ticketMapper;

        public GetAllTicketsQueryHandler(
            ITicketRepository ticketRepository,
            Mapper<Ticket, TicketResponse> ticketMapper)
        {
            _ticketRepository = ticketRepository;
            _ticketMapper = ticketMapper;
        }

        public async Task<Result<IEnumerable<TicketResponse>>> Handle(
            GetAllTicketsQuery request,
            CancellationToken cancellationToken)
        {
            var tickets = await _ticketRepository.GetAllAsync(
                cancellationToken: cancellationToken);

            return Result.Success(_ticketMapper.Map(tickets));
        }
    }
}
