using Microsoft.EntityFrameworkCore;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Infrastructure.Database;

namespace TraffiLearn.Infrastructure.Repositories
{
    public sealed class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        #region Generic Methods


        public async Task AddAsync(DrivingCategory item)
        {
            await _dbContext.DrivingCategories.AddAsync(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid key)
        {
            var found = await GetByIdAsync(key);

            _dbContext.DrivingCategories.Remove(found);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid key)
        {
            return (await _dbContext.DrivingCategories.FindAsync(key)) is not null;
        }

        public async Task<IEnumerable<DrivingCategory>> GetAllAsync()
        {
            return await _dbContext.DrivingCategories.ToListAsync();
        }

        public async Task<DrivingCategory?> GetByIdAsync(Guid key)
        {
            return await _dbContext.DrivingCategories.FindAsync(key);
        }

        public async Task UpdateAsync(Guid key, DrivingCategory item)
        {
            var found = await GetByIdAsync(key);

            _dbContext.Entry(found).CurrentValues.SetValues(item);
            await _dbContext.SaveChangesAsync();
        }


        #endregion
    }
}
