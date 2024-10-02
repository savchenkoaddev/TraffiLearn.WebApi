using Microsoft.EntityFrameworkCore;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Topics;
using TraffiLearn.Domain.Aggregates.Topics.ValueObjects;

namespace TraffiLearn.Infrastructure.Persistence.Repositories
{
    internal sealed class TopicRepository : ITopicRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TopicRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InsertAsync(
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
            TopicId topicId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Topics.FindAsync(
                keyValues: [topicId],
                cancellationToken) is not null;
        }

        public Task UpdateAsync(Topic topic)
        {
            _dbContext.Topics.Update(topic);

            return Task.CompletedTask;
        }

        public async Task<Topic?> GetByIdAsync(
            TopicId topicId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Topics
                .FindAsync(
                    keyValues: [topicId],
                    cancellationToken);
        }

        public async Task<Topic?> GetByIdWithQuestionsAsync(
            TopicId topicId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Topics
                .Where(t => t.Id == topicId)
                .Include(t => t.Questions)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<Topic>> GetManyByQuestionIdAsync(
            QuestionId questionId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Topics
                .Where(t => t.Questions.Any(q => q.Id == questionId))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Topic>> GetAllSortedByNumberAsync(
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Topics
                .OrderBy(t => t.Number)
                .ToListAsync(cancellationToken);
        }

        public Task<Topic?> GetRandomRecordAsync(
            CancellationToken cancellationToken = default)
        {
            var sql = """
                SELECT *
                FROM "{0}"
                ORDER BY RANDOM()
                LIMIT 1
            """;

            var formattedSql = string.Format(
               sql,
               nameof(ApplicationDbContext.Topics));

            return _dbContext.Topics
                .FromSqlRaw(formattedSql)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
