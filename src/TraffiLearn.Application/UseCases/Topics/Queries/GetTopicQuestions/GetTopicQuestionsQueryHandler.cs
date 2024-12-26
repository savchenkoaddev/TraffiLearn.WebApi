using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.UseCases.Questions.DTO;
using TraffiLearn.Domain.Questions;
using TraffiLearn.Domain.Topics;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Topics.Queries.GetTopicQuestions
{
    internal sealed class GetTopicQuestionsQueryHandler 
        : IRequestHandler<GetTopicQuestionsQuery, Result<IEnumerable<QuestionResponse>>>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly Mapper<Question, QuestionResponse> _questionMapper;

        public GetTopicQuestionsQueryHandler(
            ITopicRepository topicRepository,
            IQuestionRepository questionRepository,
            Mapper<Question, QuestionResponse> questionMapper)
        {
            _topicRepository = topicRepository;
            _questionRepository = questionRepository;
            _questionMapper = questionMapper;
        }

        public async Task<Result<IEnumerable<QuestionResponse>>> Handle(
            GetTopicQuestionsQuery request,
            CancellationToken cancellationToken)
        {
            var topicId = new TopicId(request.TopicId);

            var topicExists = await _topicRepository.ExistsAsync(
                topicId,
                cancellationToken);

            if (!topicExists)
            {
                return Result.Failure<IEnumerable<QuestionResponse>>(TopicErrors.NotFound);
            }

            var questions = await _questionRepository.GetManyByTopicIdAsync(
                topicId,
                cancellationToken);

            return Result.Success(_questionMapper.Map(questions));
        }
    }
}
