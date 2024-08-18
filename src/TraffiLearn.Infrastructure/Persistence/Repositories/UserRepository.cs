using Microsoft.EntityFrameworkCore;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;

namespace TraffiLearn.Infrastructure.Persistence.Repositories
{
    internal sealed class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(
            User user,
            CancellationToken cancellationToken = default)
        {
            await _dbContext.Users.AddAsync(
                user,
                cancellationToken);
        }

        public Task DeleteAsync(User user)
        {
            _dbContext.Users.Remove(user);

            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(
            UserId userId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.FindAsync(
                keyValues: [userId],
                cancellationToken: cancellationToken) is not null;
        }

        public async Task<bool> ExistsAsync(
            Username username,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(
                user => user.Username == username,
                cancellationToken) is not null;
        }

        public async Task<bool> ExistsAsync(
            Username username,
            Email email,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(
                user => user.Username == username || user.Email == email,
                cancellationToken: cancellationToken) is not null;
        }

        public Task<User?> GetByEmailAsync(
            Email email,
            CancellationToken cancellationToken = default)
        {
            return _dbContext.Users
                .FirstOrDefaultAsync(
                    user => user.Email == email,
                    cancellationToken);
        }

        public Task<User?> GetByIdAsync(
            UserId userId,
            CancellationToken cancellationToken = default)
        {
            return _dbContext.Users
                .FirstOrDefaultAsync(
                    c => c.Id == userId,
                    cancellationToken);
        }

        public Task<User?> GetByIdWithLikedAndDislikedCommentsAsync(
            UserId userId,
            CancellationToken cancellationToken = default)
        {
            return _dbContext.Users
                .Where(user => user.Id == userId)
                .Include(user => user.LikedComments)
                .Include(user => user.DislikedComments)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task<User?> GetByIdWithLikedAndDislikedQuestionsAsync(
            UserId userId,
            CancellationToken cancellationToken = default)
        {
            return _dbContext.Users
                .Where(user => user.Id == userId)
                .Include(user => user.LikedQuestions)
                .Include(user => user.DislikedQuestions)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task<User?> GetByIdWithMarkedQuestionsAsync(UserId userId, CancellationToken cancellationToken = default)
        {
            return _dbContext.Users
                .Where(user => user.Id == userId)
                .Include(user => user.MarkedQuestions)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task<User?> GetByUsernameAsync(
            Username username,
            CancellationToken cancellationToken = default)
        {
            return _dbContext.Users.FirstOrDefaultAsync(
                user => user.Username == username,
                cancellationToken);
        }

        public Task UpdateAsync(User user)
        {
            _dbContext.Users.Update(user);

            return Task.CompletedTask;
        }
    }
}
