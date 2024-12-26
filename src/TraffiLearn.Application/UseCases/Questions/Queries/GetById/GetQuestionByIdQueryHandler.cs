using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.UseCases.Questions.DTO;
using TraffiLearn.Domain.Questions;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Questions.Queries.GetById
{
    internal sealed class GetQuestionByIdQueryHandler 
        : IRequestHandler<GetQuestionByIdQuery, Result<QuestionResponse>>
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
                questionId: new QuestionId(request.QuestionId),
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
