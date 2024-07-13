using MediatR;
using TraffiLearn.Application.DTO.Questions.Response;
using TraffiLearn.Application.Questions;
using TraffiLearn.Domain.Exceptions;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Topics.Queries.GetQuestionsForTopic
{
    public sealed class GetQuestionsForTopicQueryHandler : IRequestHandler<GetQuestionsForTopicQuery, IEnumerable<QuestionResponse>>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly QuestionMapper _questionMapper = new();

        public GetQuestionsForTopicQueryHandler(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
        }

        public async Task<IEnumerable<QuestionResponse>> Handle(GetQuestionsForTopicQuery request, CancellationToken cancellationToken)
        {
            var topic = await _topicRepository.GetByIdAsync(request.TopicId.Value);

            if (topic is null)
            {
                throw new TopicNotFoundException(request.TopicId.Value);
            }

            return _questionMapper.ToResponse(topic.Questions);
        }
    }
}
