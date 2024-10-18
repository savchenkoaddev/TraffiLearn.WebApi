using System.Data;

namespace TraffiLearn.Application.Abstractions.Data
{
    public interface IUnitOfWork
    {
        Task ExecuteInTransactionAsync(
            Func<Task> action,
            IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
            CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
