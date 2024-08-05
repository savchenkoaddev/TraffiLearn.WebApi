using System.Linq.Expressions;
using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Domain.RepositoryContracts
{
    public interface IQuestionRepository
    {
        Task<Question?> GetByIdAsync(
            Guid questionId,
            CancellationToken cancellationToken = default,
            params Expression<Func<Question, object>>[] includeExpressions);

        Task<IEnumerable<Comment>> GetQuestionCommentsWithRepliesAsync(
            Guid questionId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Question>> GetAllAsync(
            Expression<Func<Question, object>>? orderByExpression = null,
            CancellationToken cancellationToken = default,
            params Expression<Func<Question, object>>[] includeExpressions);

        Task<IEnumerable<Question>> GetRandomRecordsAsync(
            int amount,
            Expression<Func<Question, object>>? orderByExpression = null,
            CancellationToken cancellationToken = default,
            params Expression<Func<Question, object>>[] includeExpressions);

        Task<bool> ExistsAsync(
            Guid questionId,
            CancellationToken cancellationToken = default);

        Task AddAsync(
            Question question,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(Question question);

        Task DeleteAsync(Question question);
    }
}
