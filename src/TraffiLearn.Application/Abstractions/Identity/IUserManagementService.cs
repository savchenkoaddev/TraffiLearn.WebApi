using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Abstractions.Identity
{
    public interface IUserManagementService
    {
        Task<Result> CreateUserAsync(
            User user,
            string password,
            CancellationToken cancellationToken = default);

        Task<Result> DeleteUserAsync(
            UserId userId,
            CancellationToken cancellationToken = default);

        Task<Result> UpdateUserRoleAsync(
            User user,
            CancellationToken cancellationToken = default);

        Task<Result<User>> GetAuthenticatedUserAsync(
            CancellationToken cancellationToken = default);
    }
}
