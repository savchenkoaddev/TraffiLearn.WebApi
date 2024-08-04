using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Topics;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Topics.GetAllSortedByNumber
{
    internal sealed class GetAllSortedTopicsByNumberQueryHandler : IRequestHandler<GetAllSortedTopicsByNumberQuery, Result<IEnumerable<TopicResponse>>>
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

        public async Task<Result<IEnumerable<TopicResponse>>> Handle(
            GetAllSortedTopicsByNumberQuery request, 
            CancellationToken cancellationToken)
        {
            var sortedTopics = await _topicRepository.GetAllSortedLazyAsync();

            return Result.Success(_topicMapper.Map(sortedTopics));
        }
    }
}
