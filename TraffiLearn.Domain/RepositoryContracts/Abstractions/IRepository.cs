namespace TraffiLearn.Domain.RepositoryContracts.Abstractions
{
    public interface IRepository<TEntity, TKey>
    {
        Task<TEntity?> GetByIdAsync(TKey key);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<bool> ExistsAsync(TKey key);

        Task AddAsync(TEntity entity);

        Task UpdateAsync(TEntity oldEntity, TEntity newEntity);

        Task DeleteAsync(TEntity entity);
    }
}
