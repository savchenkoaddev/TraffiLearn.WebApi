using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TraffiLearn.Domain.Abstractions;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Infrastructure.Database;

namespace TraffiLearn.Infrastructure.Repositories
{
    internal abstract class Repository<TEntity> : IGenericRepository<TEntity, Guid>
        where TEntity : Entity
    {
        protected readonly ApplicationDbContext DbContext;

        protected Repository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task AddAsync(TEntity entity)
        {
            await DbContext.AddAsync(entity);
        }

        public Task DeleteAsync(TEntity entity)
        {
            DbContext.Remove(entity);

            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid key)
        {
            return (await DbContext.Set<TEntity>().FindAsync(key)) is not null;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await DbContext.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(Guid key, Expression<Func<TEntity, object>> includeExpression = null)
        {
            var query = DbContext.Set<TEntity>().AsQueryable();

            if (includeExpression != null)
            {
                query = query.Include(includeExpression);
            }

            return await query.FirstOrDefaultAsync(e => e.Id == key);
        }

        public Task UpdateAsync(TEntity entity)
        {
            DbContext.Set<TEntity>().Update(entity);

            return Task.CompletedTask;
        }
    }
}
