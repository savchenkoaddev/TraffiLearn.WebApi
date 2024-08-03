using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Domain.RepositoryContracts
{
    public interface IQuestionRepository
    {
        Task<Question?> GetByIdRawAsync(Guid questionId);

        Task<Question?> GetByIdWithTicketsAsync(Guid questionId);

        Task<Question?> GetByIdWithTopicsAsync(Guid questionId);

        Task<Question?> GetByIdWithCommentsAsync(Guid questionId);

        Task<Question?> GetByIdWithCommentsTwoLevelsDeepAsync(Guid questionId);

        Task<IEnumerable<Question>> GetAllAsync();

        Task<IEnumerable<Question>> GetRawRandomRecordsAsync(int amount);

        Task<bool> ExistsAsync(Guid questionId);

        Task AddAsync(Question question);

        Task UpdateAsync(Question question);

        Task DeleteAsync(Question question);
    }
}
