using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Queries.Topics.GetQuestionsForTopic
{
    public sealed class GetQuestionsForTopicQueryHandler : IRequestHandler<GetQuestionsForTopicQuery, IEnumerable<QuestionResponse>>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly Mapper<Question, QuestionResponse> _questionMapper;

        public GetQuestionsForTopicQueryHandler(
            ITopicRepository topicRepository,
            Mapper<Question, QuestionResponse> questionMapper)
        {
            _topicRepository = topicRepository;
            _questionMapper = questionMapper;
        }

        public async Task<IEnumerable<QuestionResponse>> Handle(GetQuestionsForTopicQuery request, CancellationToken cancellationToken)
        {
            var topic = await _topicRepository.GetByIdAsync(
                request.TopicId,
                includeExpression: x => x.Questions);

            if (topic is null)
            {
                throw new ArgumentException("Topic has not been found");
            }

            return _questionMapper.Map(topic.Questions);
        }
    }
}
