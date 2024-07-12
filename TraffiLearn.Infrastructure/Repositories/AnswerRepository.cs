using Microsoft.EntityFrameworkCore;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Infrastructure.Database;

namespace TraffiLearn.Infrastructure.Repositories
{
    public sealed class AnswerRepository : IAnswerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AnswerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Answer entity)
        {
            await _dbContext.Answers.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Answer entity)
        {
            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid key)
        {
            return (await _dbContext.Answers.FindAsync(key)) is not null;
        }

        public async Task<IEnumerable<Answer>> GetAllAsync()
        {
            return await _dbContext.Answers.ToListAsync();
        }

        public async Task<Answer?> GetByIdAsync(Guid key)
        {
            return await _dbContext.Answers.FindAsync(key);
        }

        public async Task UpdateAsync(Answer oldEntity, Answer newEntity)
        {
            _dbContext.Entry(oldEntity).CurrentValues.SetValues(newEntity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
