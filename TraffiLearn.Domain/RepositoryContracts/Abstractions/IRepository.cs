namespace TraffiLearn.Domain.RepositoryContracts.Abstractions
{
    public interface IRepository<TEntity, TKey>
    {
        Task<TEntity?> GetByIdAsync(TKey key);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<bool> ExistsAsync(TKey key);

        Task AddAsync(TEntity item);

        Task UpdateAsync(TKey key, TEntity item);

        Task DeleteAsync(TKey key);
    }
}
