using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Questions.DomainEvents;
using TraffiLearn.Domain.Aggregates.Topics;

namespace TraffiLearn.Application.Questions.Events
{
    internal sealed class TopicAddedToQuestionDomainEventHandler
        : INotificationHandler<TopicAddedToQuestionDomainEvent>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ITopicRepository _topicRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TopicAddedToQuestionDomainEventHandler> _logger;

        public TopicAddedToQuestionDomainEventHandler(
            IQuestionRepository questionRepository,
            ITopicRepository topicRepository,
            IUnitOfWork unitOfWork,
            ILogger<TopicAddedToQuestionDomainEventHandler> logger)
        {
            _questionRepository = questionRepository;
            _topicRepository = topicRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(
            TopicAddedToQuestionDomainEvent notification, 
            CancellationToken cancellationToken)
        {
            var topic = await _topicRepository.GetByIdAsync(
                topicId: notification.TopicId,
                cancellationToken);

            if (topic is null)
            {
                _logger.LogCritical(
                    "Topic with the provided id in the notification is not found. Topic id: {id}. Domain event id: {eventId}", 
                    notification.TopicId.Value,
                    notification.Id);

                return;
            }

            var question = await _questionRepository.GetByIdAsync(
                questionId: notification.QuestionId,
                cancellationToken);

            if (question is null)
            {
                _logger.LogCritical(
                     "Question with the provided id in the notification is not found. Question id: {id}. Domain event id: {eventId}",
                     notification.QuestionId.Value,
                     notification.Id);

                return;
            }

            var result = topic.AddQuestion(question);

            if (result.IsFailure)
            {
                _logger.LogError(
                    "Failure while adding question to topic. Error: {error}. Domain event id: {eventId}",
                    result.Error.Description,
                    notification.Id);
            }
        }
    }
}
