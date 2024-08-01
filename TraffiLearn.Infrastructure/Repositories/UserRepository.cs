using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Infrastructure.Database;

namespace TraffiLearn.Infrastructure.Repositories
{
    internal sealed class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
        }

        public Task DeleteAsync(User user)
        {
            _dbContext.Users.Remove(user);

            return Task.CompletedTask;
        }

        public async Task<User?> GetByIdAsync(Guid userId)
        {
            return await _dbContext.Users.FindAsync(userId);
        }
    }
}
