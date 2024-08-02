using Microsoft.EntityFrameworkCore;
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

        public async Task<Comment?> GetByIdRawAsync(Guid commentId)
        {
            return await _dbContext.Comments.FindAsync(commentId);
        }

        public async Task<Comment?> GetByIdWithQuestionAsync(Guid commentId)
        {
            return await _dbContext.Comments
                .Include(c => c.Question)
                .FirstOrDefaultAsync(c => c.Id == commentId);
        }

        public async Task<Comment?> GetByIdWithUserAsync(Guid commentId)
        {
            return await _dbContext.Comments
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == commentId);
        }

        public Task UpdateAsync(Comment comment)
        {
            _dbContext.Comments.Update(comment);

            return Task.CompletedTask;
        }
    }
}
