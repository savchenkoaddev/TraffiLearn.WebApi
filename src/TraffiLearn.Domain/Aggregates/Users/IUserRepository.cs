﻿using TraffiLearn.Domain.Aggregates.Users.Emails;
using TraffiLearn.Domain.Aggregates.Users.Usernames;
using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.Aggregates.Users
{
    public interface IUserRepository : IRepositoryMarker
    {
        Task<User?> GetByIdAsync(
            UserId userId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<User>> GetAllAsync(
            CancellationToken cancellationToken = default);

        Task<IEnumerable<User>> GetAllAdminsAsync(
            CancellationToken cancellationToken = default);

        Task<User?> GetByIdWithLikedAndDislikedCommentsAsync(
            UserId userId,
            CancellationToken cancellationToken = default);

        Task<User?> GetByIdWithLikedAndDislikedQuestionsAsync(
            UserId userId,
            CancellationToken cancellationToken = default);

        Task<User?> GetByIdWithMarkedQuestionsAsync(
            UserId userId,
            CancellationToken cancellationToken = default);

        Task<User?> GetByEmailAsync(
            Email email,
            CancellationToken cancellationToken = default);

        Task<User?> GetByUsernameAsync(
            Username username,
            CancellationToken cancellationToken = default);

        Task InsertAsync(
            User user,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(User user);

        Task UpdateAsync(User user);

        Task<bool> ExistsAsync(
            UserId userId,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            Username username,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            Username username,
            Email email,
            CancellationToken cancellationToken = default);
    }
}
