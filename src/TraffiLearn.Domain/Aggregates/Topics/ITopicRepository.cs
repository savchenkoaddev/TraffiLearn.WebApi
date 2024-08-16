using System.Linq.Expressions;
using TraffiLearn.Domain.Aggregates.Topics.ValueObjects;

namespace TraffiLearn.Domain.Aggregates.Topics
{
    public interface ITopicRepository
    {
        Task<Topic?> GetByIdAsync(
            TopicId topicId,
            CancellationToken cancellationToken = default,
            params Expression<Func<Topic, object>>[] includeExpressions);

        Task<IEnumerable<Topic>> GetAllAsync(
            Expression<Func<Topic, object>>? orderByExpression = null,
            CancellationToken cancellationToken = default,
            params Expression<Func<Topic, object>>[] includeExpressions);

        Task<Topic?> GetRandomRecordAsync(
            CancellationToken cancellationToken = default,
            params Expression<Func<Topic, object>>[] includeExpressions);

        Task<bool> ExistsAsync(
            TopicId topicId,
            CancellationToken cancellationToken = default);

        Task AddAsync(
            Topic topic,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(Topic topic);

        Task DeleteAsync(Topic topic);
    }
}
