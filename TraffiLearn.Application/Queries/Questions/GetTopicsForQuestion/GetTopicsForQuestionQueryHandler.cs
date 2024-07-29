using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Topics;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Questions;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Queries.Questions.GetTopicsForQuestion
{
    public sealed class GetTopicsForQuestionQueryHandler : IRequestHandler<GetTopicsForQuestionQuery, Result<IEnumerable<TopicResponse>>>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly Mapper<Topic, TopicResponse> _topicMapper;

        public GetTopicsForQuestionQueryHandler(
            IQuestionRepository questionRepository,
            Mapper<Topic, TopicResponse> topicMapper)
        {
            _questionRepository = questionRepository;
            _topicMapper = topicMapper;
        }

        public async Task<Result<IEnumerable<TopicResponse>>> Handle(GetTopicsForQuestionQuery request, CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetByIdAsync(
                request.QuestionId.Value,
                includeExpression: x => x.Topics);

            if (question is null)
            {
                return Result.Failure<IEnumerable<TopicResponse>>(
                    QuestionErrors.NotFound);
            }

            return Result.Success(_topicMapper.Map(question.Topics));
        }
    }
}
