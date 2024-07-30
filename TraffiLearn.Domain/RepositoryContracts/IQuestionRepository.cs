using System.Linq.Expressions;
using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Domain.RepositoryContracts
{
    public interface IQuestionRepository
    {
        Task<Question?> GetByIdAsync(
            Guid questionId,
            Expression<Func<Question, object>>? includeExpression = null!);

        Task<IEnumerable<Question>> GetAllAsync();

        Task<bool> ExistsAsync(Guid id);

        Task AddAsync(Question question);

        Task UpdateAsync(Question question);

        Task DeleteAsync(Question question);
    }
}
