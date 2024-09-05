using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Questions.Queries.GetRandomQuestions
{
    internal sealed class GetRandomQuestionsQueryHandler
        : IRequestHandler<GetRandomQuestionsQuery,
            Result<IEnumerable<QuestionResponse>>>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly Mapper<Question, QuestionResponse> _questionMapper;

        public GetRandomQuestionsQueryHandler(
            IQuestionRepository questionRepository,
            Mapper<Question, QuestionResponse> questionMapper)
        {
            _questionRepository = questionRepository;
            _questionMapper = questionMapper;
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> Handle(
            GetRandomQuestionsQuery request,
            CancellationToken cancellationToken)
        {
            var randomQuestions = await _questionRepository.GetRandomRecordsAsync(
                request.Amount.Value,
                cancellationToken: cancellationToken);

            return Result.Success(_questionMapper.Map(randomQuestions));
        }
    }
}
