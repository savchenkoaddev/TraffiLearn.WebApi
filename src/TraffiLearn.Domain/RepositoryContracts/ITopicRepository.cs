using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Domain.RepositoryContracts
{
    public interface ITopicRepository
    {
        Task<Topic?> GetByIdRawAsync(Guid topicId);

        Task<Topic?> GetByIdWithQuestionsAsync(Guid topicId);

        Task<IEnumerable<Topic>> GetAllRawSortedByNumberAsync();

        Task<IEnumerable<Topic>> GetAllAsync();

        Task<bool> ExistsAsync(Guid topicId);

        Task AddAsync(Topic topic);

        Task UpdateAsync(Topic topic);

        Task DeleteAsync(Topic topic);
    }
}
