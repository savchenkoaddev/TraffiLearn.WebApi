using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Application.DTO.Topics;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Topics;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Topics.GetQuestionsForTopic
{
    public sealed class GetQuestionsForTopicQueryHandler : IRequestHandler<GetQuestionsForTopicQuery, Result<IEnumerable<QuestionResponse>>>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly Mapper<Question, QuestionResponse> _questionMapper;

        public GetQuestionsForTopicQueryHandler(
            ITopicRepository topicRepository,
            Mapper<Question, QuestionResponse> questionMapper)
        {
            _topicRepository = topicRepository;
            _questionMapper = questionMapper;
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> Handle(GetQuestionsForTopicQuery request, CancellationToken cancellationToken)
        {
            var topic = await _topicRepository.GetByIdAsync(
                request.TopicId.Value,
                includeExpression: x => x.Questions);

            if (topic is null)
            {
                return Result.Failure<IEnumerable<QuestionResponse>>(TopicErrors.NotFound);
            }

            return Result.Success(_questionMapper.Map(topic.Questions));
        }
    }
}
