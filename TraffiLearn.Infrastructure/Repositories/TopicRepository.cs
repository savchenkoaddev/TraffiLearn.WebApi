using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
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

        public async Task<Topic?> GetByIdAsync(
            Guid topicId,
            Expression<Func<Topic, object>>? includeExpression = null!)
        {
            if (includeExpression is null)
            {
                return await _dbContext.Topics.FindAsync(topicId);
            }

            IQueryable<Topic> query = _dbContext.Topics;

            if (includeExpression != null)
            {
                query = query.Include(includeExpression);
            }

            return await query
                .FirstOrDefaultAsync(q => q.Id == topicId);
        }

        public Task UpdateAsync(Topic topic)
        {
            _dbContext.Topics.Update(topic);

            return Task.CompletedTask;
        }
    }
}
