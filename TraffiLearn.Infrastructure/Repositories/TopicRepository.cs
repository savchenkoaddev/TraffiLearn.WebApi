using Microsoft.EntityFrameworkCore;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Infrastructure.Database;

namespace TraffiLearn.Infrastructure.Repositories
{
    public sealed class TopicRepository : ITopicRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TopicRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Topic entity)
        {
            await _dbContext.Topics.AddAsync(entity);
        }

        public async Task DeleteAsync(Topic entity)
        {
            _dbContext.Remove(entity);

            await Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid key)
        {
            return (await _dbContext.Topics.FindAsync(key)) is not null;
        }

        public async Task<IEnumerable<Topic>> GetAllAsync()
        {
            return await _dbContext.Topics.ToListAsync();
        }

        public async Task<Topic?> GetByIdAsync(Guid key)
        {
            return await _dbContext.Topics.FindAsync(key);
        }

        public async Task UpdateAsync(Topic oldEntity, Topic newEntity)
        {
            _dbContext.Entry(oldEntity).CurrentValues.SetValues(newEntity);

            await Task.CompletedTask;
        }
    }
}
