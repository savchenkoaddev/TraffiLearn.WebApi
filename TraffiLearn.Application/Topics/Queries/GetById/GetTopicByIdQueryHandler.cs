using MediatR;
using TraffiLearn.Application.DTO.Topics.Response;
using TraffiLearn.Domain.Exceptions;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Topics.Queries.GetById
{
    public sealed class GetTopicByIdQueryHandler : IRequestHandler<GetTopicByIdQuery, TopicResponse>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly TopicMapper _topicMapper;

        public GetTopicByIdQueryHandler(ITopicRepository topicRepository)
        {
            _topicRepository = topicRepository;
            _topicMapper = new TopicMapper();
        }

        public async Task<TopicResponse> Handle(GetTopicByIdQuery request, CancellationToken cancellationToken)
        {
            var found = await _topicRepository.GetByIdAsync(request.TopicId.Value);

            if (found is null)
            {
                throw new TopicNotFoundException(request.TopicId.Value);
            }

            return _topicMapper.ToResponse(found);
        }
    }
}
