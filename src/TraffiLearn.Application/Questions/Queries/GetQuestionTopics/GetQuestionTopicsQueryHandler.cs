using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Topics.DTO;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Questions.Errors;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Topics;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Questions.Queries.GetQuestionTopics
{
    internal sealed class GetQuestionTopicsQueryHandler : IRequestHandler<GetQuestionTopicsQuery, Result<IEnumerable<TopicResponse>>>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly Mapper<Topic, TopicResponse> _topicMapper;

        public GetQuestionTopicsQueryHandler(
            IQuestionRepository questionRepository,
            ITopicRepository topicRepository,
            Mapper<Topic, TopicResponse> topicMapper)
        {
            _questionRepository = questionRepository;
            _topicRepository = topicRepository;
            _topicMapper = topicMapper;
        }

        public async Task<Result<IEnumerable<TopicResponse>>> Handle(
            GetQuestionTopicsQuery request,
            CancellationToken cancellationToken)
        {
            var questionId = new QuestionId(request.QuestionId.Value);

            var questionExists = await _questionRepository.ExistsAsync(
                questionId,
                cancellationToken);

            if (!questionExists)
            {
                return Result.Failure<IEnumerable<TopicResponse>>(
                    QuestionErrors.NotFound);
            }

            var topics = await _topicRepository.GetManyByQuestionIdAsync(
                questionId,
                cancellationToken);

            return Result.Success(_topicMapper.Map(topics));
        }
    }
}
