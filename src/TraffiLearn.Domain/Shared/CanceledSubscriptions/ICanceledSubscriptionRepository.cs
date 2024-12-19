using TraffiLearn.Domain.Users;

namespace TraffiLearn.Domain.Shared.CanceledSubscriptions
{
    public interface ICanceledSubscriptionRepository
    {
        Task<IEnumerable<CanceledSubscription>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task InsertAsync(
            CanceledSubscription canceledSubscription,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<CanceledSubscription>> GetAllByUserIdWithSubscriptionsAsync(
            UserId userId,
            CancellationToken cancellationToken = default);
    }
}
