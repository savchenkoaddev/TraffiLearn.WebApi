using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.UseCases.Topics.DTO;
using TraffiLearn.Domain.Topics;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Topics.Queries.GetById
{
    internal sealed class GetTopicByIdQueryHandler : IRequestHandler<GetTopicByIdQuery, Result<TopicResponse>>
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
