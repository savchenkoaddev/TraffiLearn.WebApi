using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Topics;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Queries.Topics.GetAllSortedByNumber
{
    internal sealed class GetAllSortedTopicsByNumberQueryHandler : IRequestHandler<GetAllSortedTopicsByNumberQuery, IEnumerable<TopicResponse>>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly Mapper<Topic, TopicResponse> _topicMapper;

        public GetAllSortedTopicsByNumberQueryHandler(
            ITopicRepository topicRepository,
            Mapper<Topic, TopicResponse> topicMapper)
        {
            _topicRepository = topicRepository;
            _topicMapper = topicMapper;
        }

        public async Task<IEnumerable<TopicResponse>> Handle(
            GetAllSortedTopicsByNumberQuery request, 
            CancellationToken cancellationToken)
        {
            var topics = await _topicRepository.GetAllAsync();

            var sortedTopics = topics.OrderBy(
                x => x.Number.Value);

            return _topicMapper.Map(sortedTopics);
        }
    }
}
