using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TraffiLearn.Domain.Aggregates.Comments;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Infrastructure.Database;

namespace TraffiLearn.Infrastructure.Repositories
{
    internal sealed class QuestionRepository : IQuestionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public QuestionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(
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
            return (await _dbContext.Questions.FindAsync(
                keyValues: [questionId],
                cancellationToken)) is not null;
        }

        public async Task<IEnumerable<Question>> GetAllAsync(
            Expression<Func<Question, object>>? orderByExpression = null,
            CancellationToken cancellationToken = default,
            params Expression<Func<Question, object>>[] includeExpressions)
        {
            IQueryable<Question> query = _dbContext.Questions;

            foreach (var includeExpression in includeExpressions)
            {
                query = query.Include(includeExpression);
            }

            if (orderByExpression is not null)
            {
                query = query.OrderBy(orderByExpression);
            }

            return await query
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Question>> GetRandomRecordsAsync(
            int amount,
            Expression<Func<Question, object>>? orderByExpression = null,
            CancellationToken cancellationToken = default,
            params Expression<Func<Question, object>>[] includeExpressions)
        {
            if (amount < 1)
            {
                throw new ArgumentException("Amount of random records cannot be less than one.", nameof(amount));
            }

            //REQUIRES FURTHER OPTIMIZATION
            var sql = """
                SELECT TOP {0} *
                FROM {1}
                ORDER BY NEWID()
            """;

            var formattedSql = string.Format(
                sql,
                amount,
                nameof(ApplicationDbContext.Questions));

            var query = _dbContext.Questions
                .FromSqlRaw(formattedSql);

            foreach (var includeExpression in includeExpressions)
            {
                query = query.Include(includeExpression);
            }

            if (orderByExpression is not null)
            {
                query = query.OrderBy(orderByExpression);
            }

            var result = await query
                .ToListAsync(cancellationToken);

            return result;
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
            CancellationToken cancellationToken = default,
            params Expression<Func<Question, object>>[] includeExpressions)
        {
            var query = _dbContext.Questions.AsQueryable();

            foreach (var includeExpression in includeExpressions)
            {
                query = query.Include(includeExpression);
            }

            return await query
                .FirstOrDefaultAsync(
                    c => c.Id == questionId,
                    cancellationToken);
        }

        public async Task<IEnumerable<Comment>> GetQuestionCommentsWithRepliesAsync(
            QuestionId questionId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Comments
                .AsNoTracking()
                .Where(c => c.QuestionId.Id == questionId && c.RootComment == null)
                .Include(q => q.Replies)
                .Include(q => q.CreatorId)
                .ToListAsync(cancellationToken);
        }
    }
}
