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

        Task<Comment?> GetByIdWithRepliesWithUsersTwoLevelsDeepAsync(
            CommentId commentId,
            CancellationToken cancellationToken = default);

        Task<Comment?> GetByIdWithAllNestedRepliesAsync(
            CommentId commentId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Comment>> GetManyByQuestionIdAsync(
            QuestionId questionId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Comment>> GetUserCommentsAsync(
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
