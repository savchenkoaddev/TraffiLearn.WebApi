namespace TraffiLearn.Domain.Shared.CanceledSubscriptions
{
    public interface ICanceledSubscriptionRepository
    {
        Task<IEnumerable<CanceledSubscription>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task InsertAsync(
            CanceledSubscription canceledSubscription,
            CancellationToken cancellationToken = default);
    }
}
