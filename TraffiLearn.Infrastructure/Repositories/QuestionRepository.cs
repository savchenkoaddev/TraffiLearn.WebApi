using Microsoft.EntityFrameworkCore;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Infrastructure.Database;

namespace TraffiLearn.Infrastructure.Repositories
{
    public sealed class QuestionRepository : IQuestionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public QuestionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Question entity)
        {
            await _dbContext.Questions.AddAsync(entity);
        }

        public async Task DeleteAsync(Question entity)
        {
            _dbContext.Remove(entity);

            await Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid key)
        {
            return (await _dbContext.Questions.FindAsync(key)) is not null;
        }

        public async Task<IEnumerable<Question>> GetAllAsync()
        {
            return await _dbContext.Questions.ToListAsync();
        }

        public async Task<Question?> GetByIdAsync(Guid key)
        {
            return await _dbContext.Questions.FindAsync(key);
        }

        public async Task UpdateAsync(Question oldEntity, Question newEntity)
        {
            _dbContext.Entry(oldEntity).CurrentValues.SetValues(newEntity);

            await Task.CompletedTask;
        }
    }
}
