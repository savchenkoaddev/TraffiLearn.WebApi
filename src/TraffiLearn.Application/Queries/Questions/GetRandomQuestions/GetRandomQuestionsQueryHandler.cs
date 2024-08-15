using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Questions;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Questions.GetRandomQuestions
{
    internal sealed class GetRandomQuestionsQueryHandler : IRequestHandler<GetRandomQuestionsQuery, Result<IEnumerable<QuestionResponse>>>
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

        public async Task<Result<IEnumerable<QuestionResponse>>> Handle(GetRandomQuestionsQuery request, CancellationToken cancellationToken)
        {
            var randomQuestions = await _questionRepository.GetRandomRecordsAsync(request.Amount, cancellationToken: cancellationToken);

            return randomQuestions is null
                ? Result.Failure<IEnumerable<QuestionResponse>>(QuestionErrors.NotFound)
                : Result.Success(_questionMapper.Map(randomQuestions));
        }
    }
}
