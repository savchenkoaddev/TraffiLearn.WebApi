using System.Linq.Expressions;

namespace TraffiLearn.Domain.RepositoryContracts.Abstractions
{
    public interface IRepository<TEntity, TKey>
    {
        Task<TEntity?> GetByIdAsync(TKey key, Expression<Func<TEntity, object>> includeExpression = null);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<bool> ExistsAsync(TKey key);

        Task AddAsync(TEntity entity);

        Task UpdateAsync(TEntity oldEntity, TEntity newEntity);

        Task DeleteAsync(TEntity entity);
    }
}
