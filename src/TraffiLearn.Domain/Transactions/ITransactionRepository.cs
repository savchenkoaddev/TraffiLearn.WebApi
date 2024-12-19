using TraffiLearn.Domain.Users;

namespace TraffiLearn.Domain.Transactions
{
    public interface ITransactionRepository
    {
        Task InsertAsync(
            Transaction transaction,
            CancellationToken cancellationToken = default);

        Task<Transaction?> GetByIdWithSubscriptionPlanAsync(
            TransactionId id,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Transaction>> GetAllByUserIdWithSubscriptionPlansAsync(
            UserId userId,
            CancellationToken cancellationToken = default);
    } 
}
