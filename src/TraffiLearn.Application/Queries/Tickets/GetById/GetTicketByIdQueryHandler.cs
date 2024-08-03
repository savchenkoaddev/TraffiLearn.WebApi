using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Tickets;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Tickets;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Tickets.GetById
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
            var ticket = await _ticketRepository.GetByIdRawAsync(request.TicketId.Value);

            if (ticket is null)
            {
                return Result.Failure<TicketResponse>(TicketErrors.NotFound);
            }

            return _ticketMapper.Map(ticket);
        }
    }
}
