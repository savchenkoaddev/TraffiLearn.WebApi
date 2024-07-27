using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Topics.Response;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Exceptions;
using TraffiLearn.Domain.RepositoryContracts;

namespace TraffiLearn.Application.Questions.Queries.GetTopicsForQuestion
{
    public sealed class GetTopicsForQuestionQueryHandler : IRequestHandler<GetTopicsForQuestionQuery, IEnumerable<TopicResponse>>
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

        public async Task<IEnumerable<TopicResponse>> Handle(GetTopicsForQuestionQuery request, CancellationToken cancellationToken)
        {
            var question = await _questionRepository.GetByIdAsync(
                request.QuestionId,
                includeExpression: x => x.Topics);

            if (question is null)
            {
                throw new QuestionNotFoundException(request.QuestionId);
            }

            return _topicMapper.Map(question.Topics);
        }
    }
}
