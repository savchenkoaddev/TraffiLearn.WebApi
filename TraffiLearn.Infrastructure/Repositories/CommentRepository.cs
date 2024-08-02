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

        public async Task<Comment?> GetByIdWithAllNestedCommentsAsync(Guid commentId)
        {
            var sql = """
                WITH RecursiveComments AS (
                    SELECT 
                        Id, 
                        Content, 
                        UserId, 
                        QuestionId, 
                        RootCommentId,
                        0 AS Level
                    FROM Comments
                    WHERE Id = {0}

                    UNION ALL

                    SELECT 
                        c.Id, 
                        c.Content, 
                        c.UserId, 
                        c.QuestionId, 
                        c.RootCommentId,
                        rc.Level + 1 AS Level
                    FROM Comments c
                    INNER JOIN RecursiveComments rc ON c.RootCommentId = rc.Id
                )
                SELECT 
                    rc.Id, 
                    rc.Content, 
                    rc.UserId, 
                    rc.QuestionId, 
                    rc.RootCommentId,
                    rc.Level
                FROM RecursiveComments rc
                ORDER BY rc.Level
            """;

            var result = await _dbContext.Comments
                .FromSqlRaw(sql, commentId)
                .ToListAsync();

            var rootComment = result.FirstOrDefault();

            return rootComment;
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
