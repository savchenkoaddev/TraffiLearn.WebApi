using Microsoft.EntityFrameworkCore;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Infrastructure.Database;

namespace TraffiLearn.Infrastructure.Repositories
{
    internal sealed class TopicRepository : ITopicRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TopicRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Topic topic)
        {
            await _dbContext.Topics.AddAsync(topic);
        }

        public Task DeleteAsync(Topic topic)
        {
            _dbContext.Topics.Remove(topic);

            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return (await _dbContext.Topics.FindAsync(id)) is not null;
        }

        public async Task<IEnumerable<Topic>> GetAllAsync()
        {
            return await _dbContext.Topics.ToListAsync();
        }

        public async Task<IEnumerable<Topic>> GetAllRawSortedByNumberAsync()
        {
            return await _dbContext.Topics
                .OrderBy(t => t.Number)
                .ToListAsync();
        }

        public async Task<Topic?> GetByIdRawAsync(Guid topicId)
        {
            return await _dbContext.Topics.FindAsync(topicId);
        }

        public async Task<Topic?> GetByIdWithQuestionsAsync(Guid topicId)
        {
            return await _dbContext.Topics
                .Include(t => t.Questions)
                .FirstOrDefaultAsync(t => t.Id == topicId);
        }

        public Task UpdateAsync(Topic topic)
        {
            _dbContext.Topics.Update(topic);

            return Task.CompletedTask;
        }
    }
}
