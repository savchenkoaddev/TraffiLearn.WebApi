using System.Linq.Expressions;
using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Domain.RepositoryContracts
{
    public interface ITopicRepository
    {
        Task<Topic?> GetByIdAsync(
            Guid topicId,
            Expression<Func<Topic, object>> includeExpression = null!);

        Task<IEnumerable<Topic>> GetAllAsync();

        Task<bool> ExistsAsync(Guid id);

        Task AddAsync(Topic topic);

        Task UpdateAsync(Topic topic);

        Task DeleteAsync(Topic topic);
    }
}
