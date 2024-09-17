using MediatR;
using TraffiLearn.Domain.Aggregates.Questions.DomainEvents;

namespace TraffiLearn.Application.Questions.Events
{
    internal sealed class TopicAddedToQuestionDomainEventHandler
        : INotificationHandler<TopicAddedToQuestionDomainEvent>
    {
        public async Task Handle(
            TopicAddedToQuestionDomainEvent notification, 
            CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
        }
    }
}
