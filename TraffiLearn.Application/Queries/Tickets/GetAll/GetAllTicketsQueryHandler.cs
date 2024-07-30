using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Tickets;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Tickets.GetAll
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
            var tickets = await _ticketRepository.GetAllAsync();

            return Result.Success(_ticketMapper.Map(tickets));
        }
    }
}
