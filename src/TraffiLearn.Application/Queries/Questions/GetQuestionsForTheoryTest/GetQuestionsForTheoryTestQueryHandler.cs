using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Application.Errors;
using TraffiLearn.Application.Options;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Questions.GetQuestionsForTheoryTest
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

            if (questions.Count() < neededQuestionsCount &&
                _questionsSettings.DemandEnoughRecordsOnTheoryTestFetching)
            {
                _logger.LogError(InternalErrors.NotEnoughRecords.Description);

                return Result.Failure<IEnumerable<QuestionResponse>>(InternalErrors.NotEnoughRecords);
            }

            return Result.Success(_entityToResponseMapper.Map(questions));
        }
    }
}
