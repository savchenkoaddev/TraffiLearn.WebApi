using System.Linq.Expressions;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Tickets.ValueObjects;
using TraffiLearn.Domain.Aggregates.Topics.ValueObjects;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;

namespace TraffiLearn.Domain.Aggregates.Questions
{
    public interface IQuestionRepository
    {
        Task<Question?> GetByIdAsync(
            QuestionId questionId,
            CancellationToken cancellationToken = default);

        Task<Question?> GetByIdWithTicketsAsync(
            QuestionId questionId,
            CancellationToken cancellationToken = default);

        Task<Question?> GetByIdWithTopicsAsync(
            QuestionId questionId,
            CancellationToken cancellationToken = default);

        Task<Question?> GetByIdWithCommentsAndTheirRepliesAsync(
            QuestionId questionId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Question>> GetManyByTicketIdAsync(
            TicketId ticketId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Question>> GetManyByTopicIdAsync(
            TopicId topicId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Question>> GetUserDislikedQuestionsAsync(
            UserId userId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Question>> GetUserLikedQuestionsAsync(
            UserId userId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Question>> GetUserMarkedQuestionsAsync(
            UserId userId,
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
            QuestionId questionId,
            CancellationToken cancellationToken = default);

        Task AddAsync(
            Question question,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(Question question);

        Task DeleteAsync(Question question);
    }
}
