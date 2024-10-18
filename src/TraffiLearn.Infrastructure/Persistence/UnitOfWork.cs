using Microsoft.EntityFrameworkCore;
using System.Data;
using TraffiLearn.Application.Abstractions.Data;

namespace TraffiLearn.Infrastructure.Persistence
{
    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ExecuteInTransactionAsync(
            Func<Task> action,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            CancellationToken cancellationToken = default)
        {
            await using var transaction = await _dbContext.Database
                .BeginTransactionAsync(
                    isolationLevel: isolationLevel,
                    cancellationToken);

            try
            {
                await action();

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();

                throw;
            }
        }

        public Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            return _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
