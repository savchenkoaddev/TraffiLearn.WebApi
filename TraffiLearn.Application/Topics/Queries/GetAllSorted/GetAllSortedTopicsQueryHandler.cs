using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Topics.Response;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Topics.Queries.GetAllSorted
{
    public sealed class GetAllSortedTopicsQueryHandler : IRequestHandler<GetAllSortedTopicsQuery, IEnumerable<TopicResponse>>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly Mapper<Topic, TopicResponse> _topicMapper;

        public GetAllSortedTopicsQueryHandler(
            ITopicRepository topicRepository,
            Mapper<Topic, TopicResponse> topicMapper)
        {
            _topicRepository = topicRepository;
            _topicMapper = topicMapper;
        }

        public async Task<IEnumerable<TopicResponse>> Handle(GetAllSortedTopicsQuery request, CancellationToken cancellationToken)
        {
            var sortedTopics = (await _topicRepository.GetAllAsync())
                .OrderBy(x => x.Number);

            return _topicMapper.Map(sortedTopics);
        }
    }
}
