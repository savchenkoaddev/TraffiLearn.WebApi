using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Questions.Response;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Questions.Queries.GetAllQuestions
{
    public sealed class GetAllQuestionsQueryHandler : IRequestHandler<GetAllQuestionsQuery, IEnumerable<QuestionResponse>>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper<Question, QuestionResponse> _questionMapper;

        public GetAllQuestionsQueryHandler(
            IQuestionRepository questionRepository,
            IMapper<Question, QuestionResponse> questionMapper)
        {
            _questionRepository = questionRepository;
            _questionMapper = questionMapper;
        }

        public async Task<IEnumerable<QuestionResponse>> Handle(GetAllQuestionsQuery request, CancellationToken cancellationToken)
        {
            var questions = await _questionRepository.GetAllAsync();

            return _questionMapper.Map(questions);
        }
    }
}
