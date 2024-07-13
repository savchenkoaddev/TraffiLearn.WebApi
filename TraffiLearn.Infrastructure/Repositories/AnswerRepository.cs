using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
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
        }

        public async Task DeleteAsync(Answer entity)
        {
            _dbContext.Remove(entity);

            await Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid key)
        {
            return (await _dbContext.Answers.FindAsync(key)) is not null;
        }

        public async Task<IEnumerable<Answer>> GetAllAsync()
        {
            return await _dbContext.Answers.ToListAsync();
        }

        public async Task<Answer?> GetByIdAsync(Guid key, Expression<Func<Answer, object>> includeExpression = null)
        {
            if (includeExpression is not null)
            {
                return await _dbContext.Answers
                    .Include(includeExpression)
                    .FirstOrDefaultAsync(a => a.Id == key);
            }

            return await _dbContext.Answers.FindAsync(key);
        }

        public async Task UpdateAsync(Answer oldEntity, Answer newEntity)
        {
            _dbContext.Entry(oldEntity).CurrentValues.SetValues(newEntity);

            await Task.CompletedTask;
        }
    }
}
