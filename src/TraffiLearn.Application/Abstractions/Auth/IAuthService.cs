using Microsoft.AspNetCore.Identity;
using TraffiLearn.Domain.Enums;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Abstractions.Auth
{
    public interface IAuthService<TUser>
        where TUser : class
    {
        Result<Email> GetAuthenticatedUserEmail();

        Result<Guid> GetAuthenticatedUserId();

        Task<Result<SignInResult>> PasswordLogin(
            TUser user,
            string password);

        Task<Result> RemoveRole(
           TUser user,
           Role role);

        Task<Result> AssignRoleToUser(
            TUser user,
            Role role);

        Task<Result> AddIdentityUser(
            TUser identityUser,
            string password);

        Task<Result> DeleteUser(Guid userId);
    }
}
