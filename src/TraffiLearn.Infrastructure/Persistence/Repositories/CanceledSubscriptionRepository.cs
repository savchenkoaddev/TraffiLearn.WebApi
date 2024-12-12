using Microsoft.EntityFrameworkCore;
using TraffiLearn.Domain.Shared.CanceledSubscriptions;

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
