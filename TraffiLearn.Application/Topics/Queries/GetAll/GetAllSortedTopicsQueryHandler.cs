using MediatR;
using TraffiLearn.Application.DTO.Topics.Response;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Topics.Queries.GetAll
{
    public sealed class GetAllSortedTopicsQueryHandler : IRequestHandler<GetAllSortedTopicsQuery, IEnumerable<TopicResponse>>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly TopicMapper _topicMapper;

        public GetAllSortedTopicsQueryHandler(
            ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
            _topicMapper = new();
        }

        public async Task<IEnumerable<TopicResponse>> Handle(GetAllSortedTopicsQuery request, CancellationToken cancellationToken)
        {
            var sortedTopics = (await _topicRepository.GetAllAsync())
                .OrderBy(x => x.Number);

            return _topicMapper.ToResponse(sortedTopics);
        }
    }
}
