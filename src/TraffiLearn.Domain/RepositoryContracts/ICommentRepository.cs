using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Domain.RepositoryContracts
{
    public interface ICommentRepository
    {
        Task<Comment?> GetByIdRawAsync(Guid commentId);

        Task<Comment?> GetByIdWithAllNestedCommentsAsync(Guid commentId);

        Task<Comment?> GetByIdWithQuestionAsync(Guid commentId);

        Task<Comment?> GetByIdWithUserAsync(Guid commentId);

        Task<bool> ExistsAsync(Guid commentId);

        Task AddAsync(Comment comment);

        Task UpdateAsync(Comment comment);

        Task DeleteAsync(Comment comment);
    }
}
