using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.Aggregates.Comments
{
    public interface ICommentRepository : IRepositoryMarker
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

        Task<IEnumerable<Comment>> GetManyByQuestionIdWithRepliesAndCreatorsAsync(
            QuestionId questionId,
            int page,
            int pageSize,
            CancellationToken cancellationToken = default);

        Task<int> CountWithQuestionIdAsync(
            QuestionId questionId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Comment>> GetUserCreatedCommentsAsync(
            UserId userId,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            CommentId commentId,
            CancellationToken cancellationToken = default);

        Task InsertAsync(
            Comment comment,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(Comment comment);

        Task DeleteAsync(Comment comment);
    }
}
