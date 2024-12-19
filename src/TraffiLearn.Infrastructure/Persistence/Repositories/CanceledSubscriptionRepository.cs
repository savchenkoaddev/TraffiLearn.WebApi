using Microsoft.EntityFrameworkCore;
using TraffiLearn.Domain.Shared.CanceledSubscriptions;
using TraffiLearn.Domain.Users;

namespace TraffiLearn.Infrastructure.Persistence.Repositories
{
    internal sealed class CanceledSubscriptionRepository
        : ICanceledSubscriptionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CanceledSubscriptionRepository(
            ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<CanceledSubscription>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.CanceledSubscriptions
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<CanceledSubscription>> GetAllByUserIdWithSubscriptionsAsync(
            UserId userId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.CanceledSubscriptions
                .Where(cs => cs.UserId == userId)
                .Include(cs => cs.SubscriptionPlan)
                .ToListAsync(cancellationToken);
        }

        public async Task InsertAsync(
            CanceledSubscription canceledSubscription, 
            CancellationToken cancellationToken = default)
        {
            await _dbContext.CanceledSubscriptions
                .AddAsync(
                    canceledSubscription,
                    cancellationToken);
        }
    }
}
