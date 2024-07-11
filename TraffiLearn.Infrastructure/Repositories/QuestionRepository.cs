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

        #region Generic Methods


        public async Task AddAsync(Question item)
        {
            await _dbContext.Questions.AddAsync(item);
            await _dbContext.SaveChangesAsync();
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

        public async Task UpdateAsync(Guid key, Question item)
        {
            var found = await GetByIdAsync(key);

            _dbContext.Entry(found).CurrentValues.SetValues(item);
            await _dbContext.SaveChangesAsync();
        }


        #endregion

        public async Task<Question?> GetRandomQuestion()
        {
            int count = await _dbContext.Questions.CountAsync();

            if (count == 0)
            {
                return null;
            }

            int index = new Random().Next(count);

            return await _dbContext.Questions.Skip(index).Take(1).FirstOrDefaultAsync();
        }

        public async Task<Question?> GetRandomQuestionForCategory(Guid categoryId)
        {
            //NOT EFFECTIVE
            //TO DO: OPTIMIZE
            return await _dbContext.Questions
                .Where(q => q.DrivingCategories.Any(c => c.Id == categoryId))
                .OrderBy(q => Guid.NewGuid())
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Question>> GetQuestionsForCategory(Guid categoryId)
        {
            return await _dbContext.Questions.Where(q => q.DrivingCategories.Any(c => c.Id == categoryId)).ToListAsync();
        }
    }
}
