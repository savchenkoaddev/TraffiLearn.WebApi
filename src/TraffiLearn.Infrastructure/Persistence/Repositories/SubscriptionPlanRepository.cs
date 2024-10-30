using Microsoft.EntityFrameworkCore;
using TraffiLearn.Domain.Aggregates.SubscriptionPlans;

namespace TraffiLearn.Infrastructure.Persistence.Repositories
{
    internal sealed class SubscriptionPlanRepository : ISubscriptionPlanRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SubscriptionPlanRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task DeleteAsync(SubscriptionPlan plan)
        {
            EnsurePassedPlanIsNotNull(plan);

            _dbContext.SubscriptionPlans.Remove(plan);

            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(
            SubscriptionPlanId id,
            CancellationToken cancellationToken = default)
        {
            return await GetByIdAsync(
                id,
                cancellationToken) is not null;
        }

        public async Task<IEnumerable<SubscriptionPlan>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.SubscriptionPlans.ToListAsync(cancellationToken);
        }

        public async Task<SubscriptionPlan?> GetByIdAsync(
            SubscriptionPlanId id,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.SubscriptionPlans.FindAsync(
                keyValues: [id],
                cancellationToken);
        }

        public async Task InsertAsync(
            SubscriptionPlan plan,
            CancellationToken cancellationToken = default)
        {
            EnsurePassedPlanIsNotNull(plan);

            await _dbContext.SubscriptionPlans.AddAsync(
                plan,
                cancellationToken);
        }

        public Task UpdateAsync(SubscriptionPlan plan)
        {
            EnsurePassedPlanIsNotNull(plan);

            _dbContext.SubscriptionPlans.Update(plan);

            return Task.CompletedTask;
        }

        private static void EnsurePassedPlanIsNotNull(SubscriptionPlan subscriptionPlan)
        {
            ArgumentNullException.ThrowIfNull(subscriptionPlan);
        }
    }
}
