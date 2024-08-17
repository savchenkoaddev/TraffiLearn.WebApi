using TraffiLearn.Domain.Aggregates.Comments.ValueObjects;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;

namespace TraffiLearn.Domain.Aggregates.Comments
{
    public interface ICommentRepository
    {
        Task<Comment?> GetByIdAsync(
            CommentId commentId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Comment>> GetRepliesWithNextRepliesByIdAsync(
            CommentId commentId,
            CancellationToken cancellationToken = default);

        Task<Comment?> GetByIdWithAllNestedRepliesAsync(
            CommentId commentId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Comment>> GetManyByQuestionIdWithRepliesAsync(
            QuestionId questionId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Comment>> GetUserCreatedCommentsAsync(
            UserId userId,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            CommentId commentId,
            CancellationToken cancellationToken = default);

        Task AddAsync(
            Comment comment,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(Comment comment);

        Task DeleteAsync(Comment comment);
    }
}
