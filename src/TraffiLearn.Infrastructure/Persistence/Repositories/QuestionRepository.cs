using Microsoft.EntityFrameworkCore;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Tickets.ValueObjects;
using TraffiLearn.Domain.Aggregates.Topics.ValueObjects;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;

namespace TraffiLearn.Infrastructure.Persistence.Repositories
{
    internal sealed class QuestionRepository : IQuestionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public QuestionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InsertAsync(
            Question question,
            CancellationToken cancellationToken = default)
        {
            await _dbContext.Questions.AddAsync(
                question,
                cancellationToken);
        }

        public async Task<bool> ExistsAsync(
            QuestionId questionId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Questions.FindAsync(
                keyValues: [questionId],
                cancellationToken) is not null;
        }

        public Task DeleteAsync(Question question)
        {
            _dbContext.Questions.Remove(question);

            return Task.CompletedTask;
        }

        public Task UpdateAsync(Question question)
        {
            _dbContext.Questions.Update(question);

            return Task.CompletedTask;
        }

        public async Task<Question?> GetByIdAsync(
            QuestionId questionId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Questions
                .FindAsync(
                    keyValues: [questionId],
                    cancellationToken);
        }

        public async Task<Question?> GetByIdWithTicketsAsync(
            QuestionId questionId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Questions
                .Where(q => q.Id == questionId)
                .Include(q => q.Tickets)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Question?> GetByIdWithTopicsAsync(
            QuestionId questionId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Questions
                .Where(q => q.Id == questionId)
                .Include(q => q.Topics)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<Question>> GetManyByTicketIdAsync(
            TicketId ticketId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Questions
                .Where(q => q.Tickets.Any(t => t.Id == ticketId))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Question>> GetManyByTopicIdAsync(
            TopicId topicId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Questions
                .Where(q => q.Topics.Any(t => t.Id == topicId))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Question>> GetUserDislikedQuestionsAsync(
            UserId userId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Questions
                .Where(q => q.DislikedByUsers.Any(user => user.Id == userId))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Question>> GetUserLikedQuestionsAsync(
            UserId userId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Questions
                .Where(q => q.LikedByUsers.Any(user => user.Id == userId))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Question>> GetUserMarkedQuestionsAsync(
            UserId userId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Questions
                .Where(q => q.MarkedByUsers.Any(user => user.Id == userId))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Question>> GetAllAsync(
            int page,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(page, 0);
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(pageSize, 0);

            return await _dbContext.Questions
                .OrderBy(q => q.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Question>> GetRandomRecordsAsync(
            int amount,
            CancellationToken cancellationToken = default)
        {
            if (amount < 1)
            {
                throw new ArgumentException("Amount of random records cannot be less than one.", nameof(amount));
            }

            //REQUIRES FURTHER OPTIMIZATION
            var sql = """
                SELECT *
                FROM "{1}"
                ORDER BY RANDOM()
                LIMIT {0}
            """;

            var formattedSql = string.Format(
                sql,
                amount,
                nameof(ApplicationDbContext.Questions));

            return await _dbContext.Questions
                .FromSqlRaw(formattedSql)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Questions.CountAsync(cancellationToken);
        }
    }
}
