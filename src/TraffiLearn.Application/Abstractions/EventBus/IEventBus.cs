namespace TraffiLearn.Application.Abstractions.EventBus
{
    public interface IEventBus
    {
        Task PublishAsync<T>(
            T message, 
            CancellationToken cancellationToken = default) 
            where T : class;
    }
}
