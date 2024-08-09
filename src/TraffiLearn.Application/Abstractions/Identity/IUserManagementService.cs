using System.Linq.Expressions;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Enums;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Abstractions.Identity
{
    public interface IUserManagementService
    {
        Task<Result> EnsureCallerCanModifyDomainObjects(
            CancellationToken cancellationToken = default);

        Task<Result> CreateUserAsync(
            User user,
            string password,
            CancellationToken cancellationToken = default);

        Task<Result> DeleteUserAsync(
            User user,
            CancellationToken cancellationToken = default);

        Task<Result> UpdateIdentityUserRoleAsync(
            User user,
            CancellationToken cancellationToken = default);

        Task<Result<User>> GetAuthenticatedUserAsync(
            CancellationToken cancellationToken = default,
            params Expression<Func<User, object>>[] includeExpressions);
    }
}
