using Microsoft.EntityFrameworkCore;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Topics;
using TraffiLearn.Domain.Aggregates.Topics.ValueObjects;
using TraffiLearn.Infrastructure.Persistance;

namespace TraffiLearn.Infrastructure.Persistance.Repositories
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

        public async Task<Topic?> GetByIdWithQuestionsIdsAsync(
            TopicId topicId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Topics
                .Where(t => t.Id == topicId)
                .Include(t => t.QuestionsIds)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<Topic>> GetManyByQuestionIdAsync(
            QuestionId questionId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Topics
                .Where(t => t.QuestionsIds.Contains(questionId))
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
                SELECT TOP 1 *
                FROM {0}
                ORDER BY NEWID()
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
