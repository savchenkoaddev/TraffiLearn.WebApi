using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.UseCases.Questions.DTO;
using TraffiLearn.Domain.Questions;
using TraffiLearn.Domain.Tickets;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Tickets.Queries.GetTicketQuestions
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
