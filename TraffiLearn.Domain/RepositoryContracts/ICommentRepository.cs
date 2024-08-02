using System.Linq.Expressions;
using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Domain.RepositoryContracts
{
    public interface ICommentRepository
    {
        Task<Comment?> GetByIdAsync(
            Guid commentId,
            Expression<Func<Comment, object>>? includeExpression = null!);

        Task<bool> ExistsAsync(Guid id);

        Task AddAsync(Comment comment);

        Task UpdateAsync(Comment comment);

        Task DeleteAsync(Comment comment);
    }
}
