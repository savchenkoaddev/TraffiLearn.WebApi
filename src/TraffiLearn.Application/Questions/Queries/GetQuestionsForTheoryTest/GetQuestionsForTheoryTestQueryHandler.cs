using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Exceptions;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Application.Questions.Options;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Questions.Queries.GetQuestionsForTheoryTest
{
    internal sealed class GetQuestionsForTheoryTestQueryHandler : IRequestHandler<GetQuestionsForTheoryTestQuery, Result<IEnumerable<QuestionResponse>>>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly QuestionsSettings _questionsSettings;
        private readonly Mapper<Question, QuestionResponse> _entityToResponseMapper;
        private readonly ILogger<GetQuestionsForTheoryTestQueryHandler> _logger;

        public GetQuestionsForTheoryTestQueryHandler(
            IQuestionRepository questionRepository,
            IOptions<QuestionsSettings> questionsSettings,
            Mapper<Question, QuestionResponse> questionMapper,
            ILogger<GetQuestionsForTheoryTestQueryHandler> logger)
        {
            _questionRepository = questionRepository;
            _questionsSettings = questionsSettings.Value;
            _entityToResponseMapper = questionMapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> Handle(
            GetQuestionsForTheoryTestQuery request,
            CancellationToken cancellationToken)
        {
            var neededQuestionsCount = _questionsSettings.TheoryTestQuestionsCount;

            var questions = await _questionRepository.GetRandomRecordsAsync(
                amount: neededQuestionsCount);

            if (NotEnoughQuestions(questions) &&
                SufficientRecordsRequired())
            {
                throw new InsufficientRecordsException(
                    requiredRecords: neededQuestionsCount,
                    availableRecords: questions.Count());
            }

            return Result.Success(_entityToResponseMapper.Map(questions));
        }

        private bool NotEnoughQuestions(IEnumerable<Question> questions)
        {
            return questions.Count() < _questionsSettings.TheoryTestQuestionsCount;
        }

        private bool SufficientRecordsRequired()
        {
            return _questionsSettings.DemandEnoughRecordsOnTheoryTestFetching;
        }
    }
}
