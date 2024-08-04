using System.Linq.Expressions;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Domain.RepositoryContracts
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(
            Guid userId,
            CancellationToken cancellationToken = default,
            params Expression<Func<User, object>>[] includeExpressions);

        Task<User?> GetByEmailAsync(
            Email email,
            CancellationToken cancellationToken = default,
            params Expression<Func<User, object>>[] includeExpressions);

        Task AddAsync(
            User user,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(User user);
    }
}
