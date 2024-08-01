using MediatR;
using Microsoft.Extensions.Options;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Application.Options;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Questions;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Questions.GetQuestionsForTheoryTest
{
    internal sealed class GetQuestionsForTheoryTestQueryHandler : IRequestHandler<GetQuestionsForTheoryTestQuery, Result<IEnumerable<QuestionResponse>>>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly QuestionsSettings _questionsSettings;
        private readonly Mapper<Question, QuestionResponse> _entityToResponseMapper;

        public GetQuestionsForTheoryTestQueryHandler(
            IQuestionRepository questionRepository,
            IOptions<QuestionsSettings> questionsSettings,
            Mapper<Question, QuestionResponse> questionMapper)
        {
            _questionRepository = questionRepository;
            _questionsSettings = questionsSettings.Value;
            _entityToResponseMapper = questionMapper;
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> Handle(
            GetQuestionsForTheoryTestQuery request,
            CancellationToken cancellationToken)
        {
            var neededQuestionsCount = _questionsSettings.TheoryTestQuestionsCount;

            var questions = await _questionRepository.GetRandomRecords(
                amount: neededQuestionsCount);

            if (questions.Count() < neededQuestionsCount &&
                _questionsSettings.DemandEnoughRecordsOnTheoryTestFetching)
            {
                return Result.Failure<IEnumerable<QuestionResponse>>(QuestionErrors.NotEnoughRecords);
            }

            return Result.Success(_entityToResponseMapper.Map(questions));
        }
    }
}
