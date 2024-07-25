using MediatR;
using TraffiLearn.Application.Abstractions;
using TraffiLearn.Application.DTO.Questions.Response;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Exceptions;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Questions.Queries.GetQuestionById
{
    public sealed class GetQuestionByIdQueryHandler : IRequestHandler<GetQuestionByIdQuery, QuestionResponse>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper<Question, QuestionResponse> _questionMapper;

        public GetQuestionByIdQueryHandler(
            IQuestionRepository questionRepository, 
            IMapper<Question, QuestionResponse> questionMapper)
        {
            _questionRepository = questionRepository;
            _questionMapper = questionMapper;
        }

        public async Task<QuestionResponse> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetByIdAsync(request.QuestionId.Value);

            if (question is null)
            {
                throw new QuestionNotFoundException(request.QuestionId.Value);
            }

            return _questionMapper.Map(question);
        }
    }
}
