using System.Linq.Expressions;
using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Domain.RepositoryContracts
{
    public interface ICommentRepository
    {
        Task<Comment?> GetByIdAsync(
            Guid commentId,
            CancellationToken cancellationToken = default,
            params Expression<Func<Comment, object>>[] includeExpressions);

        Task<Comment?> GetByIdWithRepliesTwoLevelsDeepAsync(
            Guid commentId,
            CancellationToken cancellationToken = default,
            params Expression<Func<Comment, object>>[] includeExpressions);

        Task<Comment?> GetByIdWithAllNestedRepliesAsync(
            Guid commentId,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            Guid commentId,
            CancellationToken cancellationToken = default);

        Task AddAsync(
            Comment comment,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(Comment comment);

        Task DeleteAsync(Comment comment);
    }
}
