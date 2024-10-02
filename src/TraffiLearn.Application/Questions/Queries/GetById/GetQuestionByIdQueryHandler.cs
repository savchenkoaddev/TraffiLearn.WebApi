using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Questions.Errors;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Questions.Queries.GetById
{
    internal sealed class GetQuestionByIdQueryHandler : IRequestHandler<GetQuestionByIdQuery, Result<QuestionResponse>>
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

        public async Task<Result<QuestionResponse>> Handle(
            GetQuestionByIdQuery request,
            CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetByIdAsync(
                questionId: new QuestionId(request.QuestionId.Value),
                cancellationToken);

            if (question is null)
            {
                return Result.Failure<QuestionResponse>(
                    QuestionErrors.NotFound);
            }

            return _questionMapper.Map(question);
        }
    }
}
