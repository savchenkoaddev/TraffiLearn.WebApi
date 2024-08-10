using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Abstractions.Identity
{
    public interface IAuthenticationService<TUser, TUserId>
    {
        Task<Result<string>> LoginAsync(
            string email,
            string password,
            CancellationToken cancellationToken = default);

        Task<Result<TUser>> GetAuthenticatedUserAsync(
            CancellationToken cancellationToken = default);

        Task<Result<TUserId>> GetAuthenticatedUserIdAsync();
    }
}
