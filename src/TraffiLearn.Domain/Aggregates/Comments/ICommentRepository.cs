using System.Linq.Expressions;
using TraffiLearn.Domain.Aggregates.Comments.ValueObjects;

namespace TraffiLearn.Domain.Aggregates.Comments
{
    public interface ICommentRepository
    {
        Task<Comment?> GetByIdAsync(
            CommentId commentId,
            CancellationToken cancellationToken = default,
            params Expression<Func<Comment, object>>[] includeExpressions);

        Task<Comment?> GetByIdWithRepliesWithUsersTwoLevelsDeepAsync(
            CommentId commentId,
            CancellationToken cancellationToken = default);

        Task<Comment?> GetByIdWithAllNestedRepliesAsync(
            CommentId commentId,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            CommentId commentId,
            CancellationToken cancellationToken = default);

        Task AddAsync(
            Comment comment,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(Comment comment);

        Task DeleteAsync(Comment comment);
    }
}
