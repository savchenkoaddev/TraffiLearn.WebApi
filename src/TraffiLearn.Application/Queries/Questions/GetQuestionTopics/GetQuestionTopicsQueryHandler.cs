using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Topics;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Questions.Errors;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Topics;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Questions.GetQuestionTopics
{
    internal sealed class GetQuestionTopicsQueryHandler : IRequestHandler<GetQuestionTopicsQuery, Result<IEnumerable<TopicResponse>>>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly Mapper<Topic, TopicResponse> _topicMapper;

        public GetQuestionTopicsQueryHandler(
            IQuestionRepository questionRepository,
            Mapper<Topic, TopicResponse> topicMapper)
        {
            _questionRepository = questionRepository;
            _topicMapper = topicMapper;
        }

        public async Task<Result<IEnumerable<TopicResponse>>> Handle(
            GetQuestionTopicsQuery request,
            CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetByIdAsync(
                questionId: new QuestionId(request.QuestionId.Value),
                cancellationToken,
                includeExpressions: question => question.TopicIds);

            if (question is null)
            {
                return Result.Failure<IEnumerable<TopicResponse>>(
                    QuestionErrors.NotFound);
            }

            return Result.Success(_topicMapper.Map(question.TopicIds));
        }
    }
}
