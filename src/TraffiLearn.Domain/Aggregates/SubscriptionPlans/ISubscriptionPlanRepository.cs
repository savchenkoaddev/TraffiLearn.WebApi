namespace TraffiLearn.Domain.Aggregates.SubscriptionPlans
{
    public interface ISubscriptionPlanRepository
    {
        Task<IEnumerable<SubscriptionPlan>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<SubscriptionPlan?> GetByIdAsync(
            SubscriptionPlanId id,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            SubscriptionPlanId id,
            CancellationToken cancellationToken = default);

        Task InsertAsync(
            SubscriptionPlan plan,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(SubscriptionPlan plan);

        Task DeleteAsync(SubscriptionPlan plan);
    }
}
