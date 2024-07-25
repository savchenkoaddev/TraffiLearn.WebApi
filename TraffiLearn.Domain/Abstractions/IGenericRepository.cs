using System.Linq.Expressions;
using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.Abstractions
{
    public interface IGenericRepository<TEntity, TKey>
        where TEntity : Entity
    {
        Task<TEntity?> GetByIdAsync(
            TKey key, 
            Expression<Func<TEntity, object>> includeExpression = null!);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<bool> ExistsAsync(TKey key);

        Task AddAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);
    }
}
