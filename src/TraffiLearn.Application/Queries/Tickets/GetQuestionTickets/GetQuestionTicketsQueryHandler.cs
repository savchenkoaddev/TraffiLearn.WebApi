using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Tickets;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Questions.Errors;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Tickets;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Tickets.GetQuestionTickets
{
    internal sealed class GetQuestionTicketsQueryHandler
        : IRequestHandler<GetQuestionTicketsQuery, Result<IEnumerable<TicketResponse>>>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly Mapper<Ticket, TicketResponse> _ticketMapper;

        public GetQuestionTicketsQueryHandler(
            ITicketRepository ticketRepository,
            IQuestionRepository questionRepository,
            Mapper<Ticket, TicketResponse> ticketMapper)
        {
            _ticketRepository = ticketRepository;
            _questionRepository = questionRepository;
            _ticketMapper = ticketMapper;
        }

        public async Task<Result<IEnumerable<TicketResponse>>> Handle(
            GetQuestionTicketsQuery request,
            CancellationToken cancellationToken)
        {
            var questionId = new QuestionId(request.QuestionId.Value);

            var exists = await _questionRepository.ExistsAsync(
                questionId,
                cancellationToken);

            if (!exists)
            {
                return Result.Failure<IEnumerable<TicketResponse>>(QuestionErrors.NotFound);
            }

            var tickets = await _ticketRepository.GetManyByQuestionIdAsync(
                questionId,
                cancellationToken);

            return Result.Success(_ticketMapper.Map(tickets));
        }
    }
}
