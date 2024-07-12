using MediatR;
using TraffiLearn.Application.DTO.Topics.Response;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Topics.Queries.GetAll
{
    public sealed class GetAllTopicsQueryHandler : IRequestHandler<GetAllTopicsQuery, IEnumerable<TopicResponse>>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly TopicMapper _topicMapper;

        public GetAllTopicsQueryHandler(
            ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
            _topicMapper = new();
        }

        public async Task<IEnumerable<TopicResponse>> Handle(GetAllTopicsQuery request, CancellationToken cancellationToken)
        {
            return _topicMapper.ToResponse(await _topicRepository.GetAllAsync());
        }
    }
}
