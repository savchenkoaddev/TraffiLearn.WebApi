using Microsoft.AspNetCore.Identity;
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
    }
}
