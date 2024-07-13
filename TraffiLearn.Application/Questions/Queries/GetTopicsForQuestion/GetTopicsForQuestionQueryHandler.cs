using MediatR;
using TraffiLearn.Application.DTO.Topics.Response;
using TraffiLearn.Application.Topics;
using TraffiLearn.Domain.Exceptions;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Questions.Queries.GetTopicsForQuestion
{
    public sealed class GetTopicsForQuestionQueryHandler : IRequestHandler<GetTopicsForQuestionQuery, IEnumerable<TopicResponse>>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly TopicMapper _topicMapper = new();

        public GetTopicsForQuestionQueryHandler(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;
        }

        public async Task<IEnumerable<TopicResponse>> Handle(GetTopicsForQuestionQuery request, CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetByIdAsync(request.QuestionId.Value);

            if (question is null)
            {
                throw new QuestionNotFoundException(request.QuestionId.Value);
            }

            return _topicMapper.ToResponse(question.Topics);
        }
    }
}
