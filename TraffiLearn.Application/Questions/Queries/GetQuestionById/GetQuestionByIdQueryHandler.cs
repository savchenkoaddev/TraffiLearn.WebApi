using MediatR;
using TraffiLearn.Application.DTO.Questions.Response;
using TraffiLearn.Domain.Exceptions;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Questions.Queries.GetQuestionById
{
    public sealed class GetQuestionByIdQueryHandler : IRequestHandler<GetQuestionByIdQuery, QuestionResponse>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly QuestionMapper _questionMapper = new();

        public GetQuestionByIdQueryHandler(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<QuestionResponse> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetByIdAsync(request.QuestionId.Value);

            if (question is null)
            {
                throw new QuestionNotFoundException(request.QuestionId.Value);
            }

            return _questionMapper.ToResponse(question);
        }
    }
}
