using MediatR;
using TraffiLearn.Application.Abstractions;
using TraffiLearn.Application.DTO.Topics.Response;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Exceptions;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Questions.Queries.GetTopicsForQuestion
{
    public sealed class GetTopicsForQuestionQueryHandler : IRequestHandler<GetTopicsForQuestionQuery, IEnumerable<TopicResponse>>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper<Topic, TopicResponse> _topicMapper;

        public GetTopicsForQuestionQueryHandler(
            IQuestionRepository questionRepository, 
            IMapper<Topic, TopicResponse> topicMapper)
        {
            _questionRepository = questionRepository;
            _topicMapper = topicMapper;
        }

        public async Task<IEnumerable<TopicResponse>> Handle(GetTopicsForQuestionQuery request, CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetByIdAsync(
                request.QuestionId.Value,
                includeExpression: x => x.Topics);

            if (question is null)
            {
                throw new QuestionNotFoundException(request.QuestionId.Value);
            }

            return _topicMapper.Map(question.Topics);
        }
    }
}
