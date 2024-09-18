using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Aggregates.Questions.DomainEvents;

namespace TraffiLearn.Application.Questions.Events
{
    internal sealed class TopicAddedToQuestionDomainEventHandler
        : INotificationHandler<TopicAddedToQuestionDomainEvent>
    {
        private readonly IUnitOfWork _unitOfWork;

        public TopicAddedToQuestionDomainEventHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(
            TopicAddedToQuestionDomainEvent notification, 
            CancellationToken cancellationToken)
        {
            var result = notification.Topic.AddQuestion(
                notification.Question);

            await Task.CompletedTask;
        }
    }
}
