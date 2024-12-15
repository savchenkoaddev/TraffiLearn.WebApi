using MassTransit;
using TraffiLearn.Application.Abstractions.EventBus;

namespace TraffiLearn.Infrastructure.MessageBroker
{
    internal sealed class EventBus : IEventBus
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public EventBus(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public Task PublishAsync<T>(
            T message, 
            CancellationToken cancellationToken = default)
            where T : class
        {
            return _publishEndpoint.Publish(message, cancellationToken); 
        }
    }
}
