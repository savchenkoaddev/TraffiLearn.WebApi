using Microsoft.EntityFrameworkCore;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Infrastructure.Database;

namespace TraffiLearn.Infrastructure.Repositories
{
    public class QuestionsRepository : IQuestionsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public QuestionsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Question item)
        {
            await _dbContext.Questions.AddAsync(item);
        }

        public async Task DeleteAsync(Guid key)
        {
            var found = await GetByIdAsync(key);

            _dbContext.Questions.Remove(found);
            await _dbContext.SaveChangesAsync();
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

        public async Task<Question?> GetRandomQuestionAsync()
        {
            var count = await _dbContext.Questions.CountAsync();

            if (count == 0)
            {
                return null;
            }

            var randomIndex = new Random().Next(0, count);

            return await _dbContext.Questions.Skip(randomIndex).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Guid key, Question item)
        {
            var found = await GetByIdAsync(key);

            _dbContext.Entry(found).CurrentValues.SetValues(item);
            await _dbContext.SaveChangesAsync();
        }
    }
}
