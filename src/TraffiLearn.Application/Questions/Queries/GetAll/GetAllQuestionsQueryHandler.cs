using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Questions.Queries.GetAll
{
    internal sealed class GetAllQuestionsQueryHandler : IRequestHandler<GetAllQuestionsQuery, Result<IEnumerable<QuestionResponse>>>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly Mapper<Question, QuestionResponse> _questionMapper;

        public GetAllQuestionsQueryHandler(
            IQuestionRepository questionRepository,
            Mapper<Question, QuestionResponse> questionMapper)
        {
            _questionRepository = questionRepository;
            _questionMapper = questionMapper;
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> Handle(
            GetAllQuestionsQuery request, 
            CancellationToken cancellationToken)
        {
            var questions = await _questionRepository.GetAllAsync(
                page: request.Page,
                pageSize: request.PageSize,
                cancellationToken: cancellationToken);

            return Result.Success(_questionMapper.Map(questions));
        }
    }
}
