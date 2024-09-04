using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Topics.ValueObjects;
using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.Aggregates.Topics
{
    public interface ITopicRepository : IRepositoryMarker
    {
        Task<Topic?> GetByIdAsync(
            TopicId topicId,
            CancellationToken cancellationToken = default);

        Task<Topic?> GetByIdWithQuestionsAsync(
            TopicId topicId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Topic>> GetManyByQuestionIdAsync(
            QuestionId questionId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Topic>> GetAllSortedByNumberAsync(
            CancellationToken cancellationToken = default);

        Task<Topic?> GetRandomRecordAsync(
            CancellationToken cancellationToken = default);

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
