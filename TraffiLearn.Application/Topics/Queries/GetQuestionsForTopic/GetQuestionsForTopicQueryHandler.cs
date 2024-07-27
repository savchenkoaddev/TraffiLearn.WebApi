using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Questions.Response;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Exceptions;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Topics.Queries.GetQuestionsForTopic
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
                throw new TopicNotFoundException(request.TopicId);
            }

            return _questionMapper.Map(topic.Questions);
        }
    }
}
