using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Application.Topics.DTO;
using TraffiLearn.Domain.Questions;
using TraffiLearn.Domain.Topics;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Topics.Queries.GetRandomTopicWithQuestions
{
    internal sealed class GetRandomTopicWithQuestionsQueryHandler
        : IRequestHandler<GetRandomTopicWithQuestionsQuery,
            Result<TopicWithQuestionsResponse>>
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly Mapper<Question, QuestionResponse> _questionMapper;
        private readonly ILogger<GetRandomTopicWithQuestionsQueryHandler> _logger;

        public GetRandomTopicWithQuestionsQueryHandler(
            ITopicRepository topicRepository,
            IQuestionRepository questionRepository,
            Mapper<Question, QuestionResponse> questionMapper,
            ILogger<GetRandomTopicWithQuestionsQueryHandler> logger)
        {
            _topicRepository = topicRepository;
            _questionRepository = questionRepository;
            _questionMapper = questionMapper;
            _logger = logger;
        }

        public async Task<Result<TopicWithQuestionsResponse>> Handle(
            GetRandomTopicWithQuestionsQuery request,
            CancellationToken cancellationToken)
        {
            var randomTopic = await _topicRepository.GetRandomRecordAsync(
                cancellationToken);

            if (randomTopic is null)
            {
                _logger.LogError("Failed to get a random topic from the storage.");

                return Result.Failure<TopicWithQuestionsResponse>(Error.InternalFailure());
            }

            var questions = await _questionRepository.GetManyByTopicIdAsync(
                randomTopic.Id, cancellationToken);

            var response = new TopicWithQuestionsResponse(
                TopicId: randomTopic.Id.Value,
                TopicNumber: randomTopic.Number.Value,
                Title: randomTopic.Title.Value,
                ImageUri: randomTopic.ImageUri?.Value,
                Questions: _questionMapper.Map(questions));

            return Result.Success(response);
        }
    }
}
