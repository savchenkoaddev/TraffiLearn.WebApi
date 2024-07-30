using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Infrastructure.Database;

namespace TraffiLearn.Infrastructure.Repositories
{
    internal sealed class QuestionRepository : IQuestionRepository
    {
        private const int TheoryTestQuestionsCount = 20;
        private readonly ApplicationDbContext _dbContext;

        public QuestionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Question question)
        {
            await _dbContext.Questions.AddAsync(question);
        }

        public Task DeleteAsync(Question question)
        {
            _dbContext.Questions.Remove(question);

            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return (await _dbContext.Questions.FindAsync(id)) is not null;
        }

        public async Task<IEnumerable<Question>> GetAllAsync()
        {
            return await _dbContext.Questions.ToListAsync();
        }

        public async Task<Question?> GetByIdAsync(
            Guid questionId, 
            Expression<Func<Question, object>>? includeExpression = null!)
        {
            if (includeExpression is null)
            {
                return await _dbContext.Questions.FindAsync(questionId);
            }

            IQueryable<Question> query = _dbContext.Questions;

            if (includeExpression != null)
            {
                query = query.Include(includeExpression);
            }

            return await query
                .FirstOrDefaultAsync(q => q.Id == questionId);
        }

        public async Task<IEnumerable<Question>> GetQuestionsForTheoryTest()
        {
            return await _dbContext.Questions
                .OrderBy(q => EF.Functions.Random())
                .Take(TheoryTestQuestionsCount)
                .ToListAsync();
        }

        public Task UpdateAsync(Question question)
        {
            _dbContext.Questions.Update(question);

            return Task.CompletedTask;
        }
    }
}
