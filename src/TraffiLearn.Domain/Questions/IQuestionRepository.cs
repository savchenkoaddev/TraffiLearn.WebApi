﻿using TraffiLearn.Domain.Tickets;
using TraffiLearn.Domain.Topics;
using TraffiLearn.Domain.Users;
using TraffiLearn.SharedKernel.Primitives;

namespace TraffiLearn.Domain.Questions
{
    public interface IQuestionRepository : IRepositoryMarker
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
            int page,
            int pageSize,
            CancellationToken cancellationToken = default);

        Task<int> CountAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<Question>> GetRandomRecordsAsync(
            int amount,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            QuestionId questionId,
            CancellationToken cancellationToken = default);

        Task InsertAsync(
            Question question,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(Question question);

        Task DeleteAsync(Question question);
    }
}
