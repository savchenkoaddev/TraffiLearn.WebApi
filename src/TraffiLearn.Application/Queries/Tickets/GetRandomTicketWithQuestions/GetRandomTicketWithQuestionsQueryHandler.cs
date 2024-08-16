using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Application.DTO.Tickets;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Tickets;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Tickets.GetRandomTicketWithQuestions
{
    internal sealed class GetRandomTicketWithQuestionsQueryHandler
        : IRequestHandler<GetRandomTicketWithQuestionsQuery,
            Result<TicketWithQuestionsResponse>>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly Mapper<Question, QuestionResponse> _questionMapper;
        private readonly ILogger<GetRandomTicketWithQuestionsQueryHandler> _logger;

        public GetRandomTicketWithQuestionsQueryHandler(
            ITicketRepository ticketRepository,
            IQuestionRepository questionRepository,
            Mapper<Question, QuestionResponse> questionMapper,
            ILogger<GetRandomTicketWithQuestionsQueryHandler> logger)
        {
            _ticketRepository = ticketRepository;
            _questionRepository = questionRepository;
            _questionMapper = questionMapper;
            _logger = logger;
        }

        public async Task<Result<TicketWithQuestionsResponse>> Handle(
            GetRandomTicketWithQuestionsQuery request,
            CancellationToken cancellationToken)
        {
            var randomTicket = await _ticketRepository.GetRandomRecordAsync(
                cancellationToken);

            if (randomTicket is null)
            {
                _logger.LogError("Failed to fetch a random ticket from the storage.");

                return Result.Failure<TicketWithQuestionsResponse>(Error.InternalFailure());
            }

            var ticketQuestions = await _questionRepository
                .GetManyByTicketIdAsync(
                    randomTicket.Id,
                    cancellationToken);

            var response = new TicketWithQuestionsResponse(
                TicketId: randomTicket.Id.Value,
                TicketNumber: randomTicket.TicketNumber.Value,
                Questions: _questionMapper.Map(ticketQuestions));

            return Result.Success(response);
        }
    }
}
