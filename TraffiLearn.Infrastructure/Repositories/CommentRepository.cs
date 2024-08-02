using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Infrastructure.Database;

namespace TraffiLearn.Infrastructure.Repositories
{
    internal sealed class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CommentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Comment comment)
        {
            await _dbContext.Comments.AddAsync(comment);
        }

        public Task DeleteAsync(Comment comment)
        {
            _dbContext.Comments.Remove(comment);

            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return (await _dbContext.Comments.FindAsync(id)) is not null;
        }

        public async Task<Comment?> GetByIdAsync(Guid commentId, Expression<Func<Comment, object>>? includeExpression = null)
        {
            if (includeExpression is null)
            {
                return await _dbContext.Comments.FindAsync(commentId);
            }

            IQueryable<Comment> query = _dbContext.Comments;

            if (includeExpression != null)
            {
                query = query.Include(includeExpression);
            }

            return await query
                .FirstOrDefaultAsync(q => q.Id == commentId);
        }

        public Task UpdateAsync(Comment comment)
        {
            _dbContext.Comments.Update(comment);

            return Task.CompletedTask;
        }
    }
}
