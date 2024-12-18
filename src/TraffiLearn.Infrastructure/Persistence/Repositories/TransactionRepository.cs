using Microsoft.EntityFrameworkCore;
using TraffiLearn.Domain.Transactions;
using TraffiLearn.Domain.Users;

namespace TraffiLearn.Infrastructure.Persistence.Repositories
{
    internal sealed class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TransactionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Transaction>> GetAllByUserId(
            UserId userId, 
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Transactions
                .Where(t => t.User.Id == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task<Transaction?> GetById(
            TransactionId id, 
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Transactions
                .FindAsync(
                    keyValues: [id],
                    cancellationToken);
        }

        public async Task InsertAsync(
            Transaction transaction, 
            CancellationToken cancellationToken = default)
        {
            await _dbContext.Transactions
                .AddAsync(
                    transaction,
                    cancellationToken);
        }
    }
}
