using System.Linq.Expressions;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;

namespace TraffiLearn.Domain.Aggregates.Users
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(
            UserId userId,
            CancellationToken cancellationToken = default,
            params Expression<Func<UserId, object>>[] includeExpressions);

        Task<User?> GetByEmailAsync(
            Email email,
            CancellationToken cancellationToken = default,
            params Expression<Func<UserId, object>>[] includeExpressions);

        Task<User?> GetByUsernameAsync(
            Username username,
            CancellationToken cancellationToken = default);

        Task AddAsync(
            User user,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(UserId user);

        Task UpdateAsync(User user);

        Task<bool> ExistsAsync(
            UserId userId,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            Username username,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            Username username,
            Email email,
            CancellationToken cancellationToken = default);

        Task<UserId?> GetUserWithCommentsWithRepliesAsync(
            UserId userId,
            CancellationToken cancellationToken = default);
    }
}
