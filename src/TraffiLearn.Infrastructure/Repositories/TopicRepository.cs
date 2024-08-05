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

        public async Task AddAsync(
            Topic topic,
            CancellationToken cancellationToken = default)
        {
            await _dbContext.Topics.AddAsync(
                topic,
                cancellationToken);
        }

        public Task DeleteAsync(Topic topic)
        {
            _dbContext.Topics.Remove(topic);

            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(
            Guid topicId,
            CancellationToken cancellationToken = default)
        {
            return (await _dbContext.Topics.FindAsync(
                keyValues: [topicId],
                cancellationToken)) is not null;
        }

        public async Task<IEnumerable<Topic>> GetAllAsync(
            Expression<Func<Topic, object>>? orderByExpression = null,
            CancellationToken cancellationToken = default,
            params Expression<Func<Topic, object>>[] includeExpressions)
        {
            IQueryable<Topic> topics = _dbContext.Topics;

            foreach (var includeExpression in includeExpressions)
            {
                topics = topics.Include(includeExpression);
            }

            if (orderByExpression is not null)
            {
                topics = topics.OrderBy(orderByExpression);
            }

            return await topics
                .ToListAsync(cancellationToken);
        }

        public async Task<Topic?> GetByIdAsync(
            Guid topicId,
            CancellationToken cancellationToken = default,
            params Expression<Func<Topic, object>>[] includeExpressions)
        {
            var query = _dbContext.Topics.AsQueryable();

            foreach (var includeExpression in includeExpressions)
            {
                query = query.Include(includeExpression);
            }

            return await query
                .FirstOrDefaultAsync(
                    c => c.Id == topicId,
                    cancellationToken);
        }

        public Task UpdateAsync(Topic topic)
        {
            _dbContext.Topics.Update(topic);

            return Task.CompletedTask;
        }
    }
}
