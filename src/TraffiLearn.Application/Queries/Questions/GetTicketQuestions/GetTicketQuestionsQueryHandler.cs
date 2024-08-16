using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Tickets;
using TraffiLearn.Domain.Aggregates.Tickets.Errors;
using TraffiLearn.Domain.Aggregates.Tickets.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Questions.GetTicketQuestions
{
    internal sealed class GetTicketQuestionsQueryHandler
        : IRequestHandler<GetTicketQuestionsQuery, Result<IEnumerable<QuestionResponse>>>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly Mapper<Question, QuestionResponse> _questionMapper;

        public GetTicketQuestionsQueryHandler(
            ITicketRepository ticketRepository,
            IQuestionRepository questionRepository,
            Mapper<Question, QuestionResponse> questionMapper)
        {
            _ticketRepository = ticketRepository;
            _questionRepository = questionRepository;
            _questionMapper = questionMapper;
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> Handle(
            GetTicketQuestionsQuery request,
            CancellationToken cancellationToken)
        {
            var ticketId = new TicketId(request.TicketId.Value);

            var ticketExists = await _ticketRepository.ExistsAsync(
                ticketId,
                cancellationToken);

            if (!ticketExists)
            {
                return Result.Failure<IEnumerable<QuestionResponse>>(TicketErrors.NotFound);
            }

            var questions = await _questionRepository.GetManyByTicketIdAsync(
                ticketId,
                cancellationToken);

            return Result.Success(_questionMapper.Map(questions));
        }
    }
}
