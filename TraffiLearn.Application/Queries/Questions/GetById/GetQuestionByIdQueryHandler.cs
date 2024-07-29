using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Questions;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Questions.GetById
{
    public sealed class GetQuestionByIdQueryHandler : IRequestHandler<GetQuestionByIdQuery, Result<QuestionResponse>>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly Mapper<Question, QuestionResponse> _questionMapper;

        public GetQuestionByIdQueryHandler(
            IQuestionRepository questionRepository,
            Mapper<Question, QuestionResponse> questionMapper)
        {
            _questionRepository = questionRepository;
            _questionMapper = questionMapper;
        }

        public async Task<Result<QuestionResponse>> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetByIdAsync(request.QuestionId.Value);

            if (question is null)
            {
                return Result.Failure<QuestionResponse>(QuestionErrors.NotFound);
            }

            return _questionMapper.Map(question);
        }
    }
}
