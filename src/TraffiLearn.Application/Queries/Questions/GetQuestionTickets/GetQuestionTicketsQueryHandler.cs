using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Tickets;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Questions.Errors;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Tickets;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Questions.GetQuestionTickets
{
    internal sealed class GetQuestionTicketsQueryHandler
        : IRequestHandler<GetQuestionTicketsQuery, Result<IEnumerable<TicketResponse>>>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly Mapper<Ticket, TicketResponse> _ticketMapper;

        public GetQuestionTicketsQueryHandler(
            IQuestionRepository questionRepository,
            Mapper<Ticket, TicketResponse> ticketMapper)
        {
            _questionRepository = questionRepository;
            _ticketMapper = ticketMapper;
        }

        public async Task<Result<IEnumerable<TicketResponse>>> Handle(
            GetQuestionTicketsQuery request,
            CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetByIdAsync(
                questionId: new QuestionId(request.QuestionId.Value),
                cancellationToken,
                includeExpressions: question => question.Tickets);

            if (question is null)
            {
                return Result.Failure<IEnumerable<TicketResponse>>(QuestionErrors.NotFound);
            }

            return Result.Success(_ticketMapper.Map(question.Tickets));
        }
    }
}
