using MediatR;
using TraffiLearn.Application.Abstractions;
using TraffiLearn.Application.DTO.Questions.Response;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Exceptions;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Topics.Queries.GetQuestionsForTopic
{
    public sealed class GetQuestionsForTopicQueryHandler : IRequestHandler<GetQuestionsForTopicQuery, IEnumerable<QuestionResponse>>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IMapper<Question, QuestionResponse> _questionMapper;

        public GetQuestionsForTopicQueryHandler(
            ITopicRepository topicRepository,
            IMapper<Question, QuestionResponse> questionMapper)
        {
            _topicRepository = topicRepository;
            _questionMapper = questionMapper;
        }

        public async Task<IEnumerable<QuestionResponse>> Handle(GetQuestionsForTopicQuery request, CancellationToken cancellationToken)
        {
            var topic = await _topicRepository.GetByIdAsync(
                request.TopicId.Value,
                includeExpression: x => x.Questions);

            if (topic is null)
            {
                throw new TopicNotFoundException(request.TopicId.Value);
            }

            return _questionMapper.Map(topic.Questions);
        }
    }
}
