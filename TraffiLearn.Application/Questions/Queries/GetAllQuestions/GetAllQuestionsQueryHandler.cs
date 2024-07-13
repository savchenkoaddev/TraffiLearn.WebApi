using MediatR;
using TraffiLearn.Application.DTO.Questions.Response;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Questions.Queries.GetAllQuestions
{
    public sealed class GetAllQuestionsQueryHandler : IRequestHandler<GetAllQuestionsQuery, IEnumerable<QuestionResponse>>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly QuestionMapper _questionMapper = new();

        public GetAllQuestionsQueryHandler(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<IEnumerable<QuestionResponse>> Handle(GetAllQuestionsQuery request, CancellationToken cancellationToken)
        {
            var questions = await _questionRepository.GetAllAsync();

            return _questionMapper.ToResponse(questions);
        }
    }
}
