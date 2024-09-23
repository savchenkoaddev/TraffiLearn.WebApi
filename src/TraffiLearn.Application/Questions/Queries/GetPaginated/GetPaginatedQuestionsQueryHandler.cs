using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Questions.Queries.GetPaginated
{
    internal sealed class GetPaginatedQuestionsQueryHandler : IRequestHandler<GetPaginatedQuestionsQuery, Result<IEnumerable<PaginatedQuestionsResponse>>>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly Mapper<Question, QuestionResponse> _questionMapper;

        public GetPaginatedQuestionsQueryHandler(
            IQuestionRepository questionRepository,
            Mapper<Question, QuestionResponse> questionMapper)
        {
            _questionRepository = questionRepository;
            _questionMapper = questionMapper;
        }

        public async Task<Result<IEnumerable<PaginatedQuestionsResponse>>> Handle(
            GetPaginatedQuestionsQuery request, 
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
