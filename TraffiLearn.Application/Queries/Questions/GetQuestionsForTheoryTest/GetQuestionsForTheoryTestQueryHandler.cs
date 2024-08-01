using MediatR;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Domain.Errors.Questions;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Questions.GetQuestionsForTheoryTest
{
    internal sealed class GetQuestionsForTheoryTestQueryHandler : IRequestHandler<GetQuestionsForTheoryTestQuery, Result<IEnumerable<QuestionResponse>>>
    {
        private readonly IQuestionRepository _questionRepository;

        public GetQuestionsForTheoryTestQueryHandler(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> Handle(
            GetQuestionsForTheoryTestQuery request,
            CancellationToken cancellationToken)
        {
            var questions = await _questionRepository.GetRandomRecords(20);

            if (questions.Count() < 20 &&)
            {
                return Result.Failure<IEnumerable<QuestionResponse>>(QuestionErrors.NotEnoughRecords);
            }

            return questions.
        }
    }
}
