using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Exceptions;
using TraffiLearn.Application.UseCases.Questions.DTO;
using TraffiLearn.Application.UseCases.Questions.Options;
using TraffiLearn.Domain.Questions;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Questions.Queries.GetQuestionsForTheoryTest
{
    internal sealed class GetQuestionsForTheoryTestQueryHandler 
        : IRequestHandler<GetQuestionsForTheoryTestQuery, Result<IEnumerable<QuestionResponse>>>
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
                amount: neededQuestionsCount,
                cancellationToken);

            if (NotEnoughQuestions(questions))
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
    }
}
