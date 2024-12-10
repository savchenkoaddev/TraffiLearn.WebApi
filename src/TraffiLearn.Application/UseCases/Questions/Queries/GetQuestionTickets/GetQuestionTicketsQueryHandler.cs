using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.UseCases.Tickets.DTO;
using TraffiLearn.Domain.Questions;
using TraffiLearn.Domain.Tickets;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Questions.Queries.GetQuestionTickets
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

            var questionExists = await _questionRepository.ExistsAsync(
                questionId,
                cancellationToken);

            if (!questionExists)
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
