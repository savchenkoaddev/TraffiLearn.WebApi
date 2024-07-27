using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Topics.Response;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Exceptions;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Topics.Queries.GetById
{
    public sealed class GetTopicByIdQueryHandler : IRequestHandler<GetTopicByIdQuery, TopicResponse>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly Mapper<Topic, TopicResponse> _topicMapper;

        public GetTopicByIdQueryHandler(
            ITopicRepository topicRepository,
            Mapper<Topic, TopicResponse> topicMapper)
        {
            _topicRepository = topicRepository;
            _topicMapper = topicMapper;
        }

        public async Task<TopicResponse> Handle(GetTopicByIdQuery request, CancellationToken cancellationToken)
        {
            var found = await _topicRepository.GetByIdAsync(request.TopicId);

            if (found is null)
            {
                throw new TopicNotFoundException(request.TopicId);
            }

            return _topicMapper.Map(found);
        }
    }
}
