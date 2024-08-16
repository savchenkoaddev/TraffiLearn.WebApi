using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Topics;
using TraffiLearn.Domain.Aggregates.Topics;
using TraffiLearn.Domain.Aggregates.Topics.Errors;
using TraffiLearn.Domain.Aggregates.Topics.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Topics.GetById
{
    internal sealed class GetTopicByIdQueryHandler : IRequestHandler<GetTopicByIdQuery, Result<TopicResponse>>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly Mapper<Domain.Aggregates.Topics.Topic, TopicResponse> _topicMapper;

        public GetTopicByIdQueryHandler(
            ITopicRepository topicRepository,
            Mapper<Domain.Aggregates.Topics.Topic, TopicResponse> topicMapper)
        {
            _topicRepository = topicRepository;
            _topicMapper = topicMapper;
        }

        public async Task<Result<TopicResponse>> Handle(
            GetTopicByIdQuery request,
            CancellationToken cancellationToken)
        {
            var topic = await _topicRepository.GetByIdAsync(
                topicId: new TopicId(request.TopicId.Value),
                cancellationToken);

            if (topic is null)
            {
                return Result.Failure<TopicResponse>(TopicErrors.NotFound);
            }

            return _topicMapper.Map(topic);
        }
    }
}
