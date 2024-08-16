using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Topics;
using TraffiLearn.Domain.Aggregates.Topics;
using TraffiLearn.Domain.Aggregates.Topics.Errors;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Topics.GetRandomTopicWithQuestions
{
    internal sealed class GetRandomTopicWithQuestionsQueryHandler
        : IRequestHandler<GetRandomTopicWithQuestionsQuery,
            Result<TopicWithQuestionsResponse>>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly Mapper<Topic, TopicWithQuestionsResponse> _topicMapper;

        public GetRandomTopicWithQuestionsQueryHandler(
            ITopicRepository topicRepository,
            Mapper<Topic, TopicWithQuestionsResponse> topicMapper)
        {
            _topicRepository = topicRepository;
            _topicMapper = topicMapper;
        }

        public async Task<Result<TopicWithQuestionsResponse>> Handle(
            GetRandomTopicWithQuestionsQuery request,
            CancellationToken cancellationToken)
        {
            var randomTopic = await _topicRepository.GetRandomRecordAsync(
                cancellationToken,
                t => t.Questions);

            if (randomTopic is null)
            {
                return Result.Failure<TopicWithQuestionsResponse>(TopicErrors.NotFound);
            }

            return Result.Success(_topicMapper.Map(randomTopic));
        }
    }
}
