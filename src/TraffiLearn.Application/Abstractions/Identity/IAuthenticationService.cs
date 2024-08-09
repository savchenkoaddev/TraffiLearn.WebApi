using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Abstractions.Identity
{
    public interface IAuthenticationService<TUser>
    {
        Task<Result<string>> LoginAsync(
            string email, 
            string password,
            CancellationToken cancellationToken = default);
    }
}
