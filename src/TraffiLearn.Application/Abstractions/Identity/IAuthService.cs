using System.Linq.Expressions;
using TraffiLearn.Application.Identity;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Enums;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Abstractions.Identity
{
    public interface IAuthService
    {
        Task<Result> AssignRoleToUser(
            ApplicationUser user,
            Role role);

        Task<Result> CreateUser(
            User user,
            string password,
            CancellationToken cancellationToken = default);

        Task<Result> DeleteUser(
            UserId userId,
            CancellationToken cancellationToken = default);

        Task<Result> DeleteUser(
            User user,
            CancellationToken cancellationToken = default);

        Task<Result<User>> GetCurrentUser(
            CancellationToken cancellationToken = default,
            params Expression<Func<User, object>>[] includeExpressions);
    }
}
