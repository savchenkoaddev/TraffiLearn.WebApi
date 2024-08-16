using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Topics;
using TraffiLearn.Domain.Aggregates.Topics.Errors;
using TraffiLearn.Domain.Aggregates.Topics.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Topics.GetTopicQuestions
{
    internal sealed class GetTopicQuestionsQueryHandler : IRequestHandler<GetTopicQuestionsQuery, Result<IEnumerable<QuestionResponse>>>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly Mapper<Question, QuestionResponse> _questionMapper;

        public GetTopicQuestionsQueryHandler(
            ITopicRepository topicRepository,
            Mapper<Question, QuestionResponse> questionMapper)
        {
            _topicRepository = topicRepository;
            _questionMapper = questionMapper;
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> Handle(
            GetTopicQuestionsQuery request,
            CancellationToken cancellationToken)
        {
            var topic = await _topicRepository.GetByIdAsync(
                topicId: new TopicId(request.TopicId.Value),
                cancellationToken,
                includeExpressions: topic => topic.Questions);

            if (topic is null)
            {
                return Result.Failure<IEnumerable<QuestionResponse>>(TopicErrors.NotFound);
            }

            return Result.Success(_questionMapper.Map(topic.Questions));
        }
    }
}
