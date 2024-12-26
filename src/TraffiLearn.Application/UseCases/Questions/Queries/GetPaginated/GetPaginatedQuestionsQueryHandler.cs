using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Helpers;
using TraffiLearn.Application.UseCases.Questions.DTO;
using TraffiLearn.Domain.Questions;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Questions.Queries.GetPaginated
{
    internal sealed class GetPaginatedQuestionsQueryHandler 
        : IRequestHandler<GetPaginatedQuestionsQuery, Result<PaginatedQuestionsResponse>>
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

        public async Task<Result<PaginatedQuestionsResponse>> Handle(
            GetPaginatedQuestionsQuery request,
            CancellationToken cancellationToken)
        {
            var questions = await _questionRepository.GetAllAsync(
                page: request.Page,
                pageSize: request.PageSize,
                cancellationToken: cancellationToken);

            int questionsCount = await _questionRepository.CountAsync(
                cancellationToken);

            int totalPages = PaginationCalculator.CalculateTotalPages(
                pageSize: request.PageSize,
                itemsCount: questionsCount);

            var questionsResponse = _questionMapper.Map(questions);

            return new PaginatedQuestionsResponse(
                Questions: questionsResponse,
                TotalPages: totalPages);
        }
    }
}
