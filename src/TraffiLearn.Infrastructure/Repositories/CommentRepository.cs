using Microsoft.EntityFrameworkCore;
using TraffiLearn.Domain.Aggregates.Comments;
using TraffiLearn.Domain.Aggregates.Comments.ValueObjects;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
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

        public async Task AddAsync(
            Comment comment,
            CancellationToken cancellationToken = default)
        {
            await _dbContext.Comments.AddAsync(
                comment,
                cancellationToken);
        }

        public async Task<bool> ExistsAsync(
            CommentId commentId,
            CancellationToken cancellationToken = default)
        {
            return (await _dbContext.Comments.FindAsync(
                keyValues: [commentId],
                cancellationToken)) is not null;
        }

        public Task DeleteAsync(Comment comment)
        {
            _dbContext.Comments.Remove(comment);

            return Task.CompletedTask;
        }

        public Task UpdateAsync(Comment comment)
        {
            _dbContext.Comments.Update(comment);

            return Task.CompletedTask;
        }

        public async Task<Comment?> GetByIdWithAllNestedRepliesAsync(
            CommentId commentId,
            CancellationToken cancellationToken = default)
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
                .ToListAsync(cancellationToken);

            var rootComment = result.FirstOrDefault();

            return rootComment;
        }

        public async Task<Comment?> GetByIdAsync(
            CommentId commentId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Comments
                .FindAsync(
                    keyValues: [commentId],
                    cancellationToken);
        }

        public async Task<IEnumerable<Comment>> GetUserCreatedCommentsAsync(
            UserId userId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Comments
                .Where(c => c.CreatorId == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Comment>> GetRepliesWithNextRepliesByIdAsync(
            CommentId commentId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Comments
                .Where(c => c.Id == commentId)
                .Include(c => c.Replies)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Comment>> GetManyByQuestionIdWithRepliesAsync(
            QuestionId questionId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Comments
                .Where(c => c.QuestionId == questionId)
                .Include(c => c.Replies)
                .ToListAsync(cancellationToken);
        }
    }
}
