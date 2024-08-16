using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Tickets;
using TraffiLearn.Domain.Aggregates.Tickets;
using TraffiLearn.Domain.Aggregates.Tickets.Errors;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Tickets.GetRandomTicketWithQuestions
{
    internal sealed class GetRandomTicketWithQuestionsQueryHandler
        : IRequestHandler<GetRandomTicketWithQuestionsQuery,
            Result<TicketWithQuestionsResponse>>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly Mapper<Ticket, TicketWithQuestionsResponse> _ticketMapper;

        public GetRandomTicketWithQuestionsQueryHandler(
            ITicketRepository ticketRepository,
            Mapper<Ticket, TicketWithQuestionsResponse> ticketMapper)
        {
            _ticketRepository = ticketRepository;
            _ticketMapper = ticketMapper;
        }

        public async Task<Result<TicketWithQuestionsResponse>> Handle(
            GetRandomTicketWithQuestionsQuery request,
            CancellationToken cancellationToken)
        {
            var randomTicket = await _ticketRepository.GetRandomRecordAsync(
                cancellationToken,
                t => t.Questions);

            if (randomTicket is null)
            {
                return Result.Failure<TicketWithQuestionsResponse>(TicketErrors.NotFound);
            }

            return Result.Success(_ticketMapper.Map(randomTicket));
        }
    }
}
