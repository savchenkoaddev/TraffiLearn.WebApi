using Microsoft.EntityFrameworkCore;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;
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

        public async Task<Question?> GetByIdRawAsync(Guid questionId)
        {
            return await _dbContext.Questions.FindAsync(questionId);
        }

        public async Task<Question?> GetByIdWithCommentsAsync(Guid questionId)
        {
            return await _dbContext.Questions
                .Include(q => q.Comments)
                .FirstOrDefaultAsync(q => q.Id == questionId);
        }

        public async Task<Question?> GetByIdWithTicketsAsync(Guid questionId)
        {
            return await _dbContext.Questions
                .Include(q => q.Tickets)
                .FirstOrDefaultAsync(q => q.Id == questionId);
        }

        public async Task<Question?> GetByIdWithTopicsAsync(Guid questionId)
        {
            return await _dbContext.Questions
                .Include(q => q.Topics)
                .FirstOrDefaultAsync(q => q.Id == questionId);
        }

        public async Task<IEnumerable<Question>> GetRawRandomRecordsAsync(int amount)
        {
            return await _dbContext.Questions
                .OrderBy(q => EF.Functions.Random())
                .Take(amount)
                .ToListAsync();
        }

        public Task UpdateAsync(Question question)
        {
            _dbContext.Questions.Update(question);

            return Task.CompletedTask;
        }
    }
}
