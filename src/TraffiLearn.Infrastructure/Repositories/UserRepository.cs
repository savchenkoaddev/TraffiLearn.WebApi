using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.ValueObjects.Users;
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

        public async Task<bool> ExistsAsync(Guid userId)
        {
            return (await _dbContext.Users.FindAsync(userId)) is not null;
        }

        public Task<User?> GetByEmailAsync(
            Email email, 
            CancellationToken cancellationToken = default, 
            params Expression<Func<User, object>>[] includeExpressions)
        {
            IQueryable<User> query = _dbContext.Users;

            foreach (var includeExpression in includeExpressions)
            {
                query = query.Include(includeExpression);
            }

            return query.FirstOrDefaultAsync(
                user => user.Email == email,
                cancellationToken);
        }

        public async Task<User?> GetByIdAsync(
            Guid userId, 
            CancellationToken cancellationToken = default, 
            params Expression<Func<User, object>>[] includeExpressions)
        {
            var query = _dbContext.Users.AsQueryable();

            foreach (var includeExpression in includeExpressions)
            {
                query = query.Include(includeExpression);
            }

            return await query
                .FirstOrDefaultAsync(
                    c => c.Id == userId,
                    cancellationToken);
        }

        public async Task<IEnumerable<Comment>> GetUserCommentsWithRepliesAsync(
            Guid userId, 
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Comments
                .AsNoTracking()
                .Where(c => c.User.Id == userId)
                .Include(q => q.Replies)
                .Include(q => q.User)
                .ToListAsync(cancellationToken);
        }
    }
}
