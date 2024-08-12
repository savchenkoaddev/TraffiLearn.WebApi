using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Application.Abstractions.Identity
{
    public interface IAuthenticatedUserService
    {
        Task<User> GetAuthenticatedUserAsync(
            CancellationToken cancellationToken = default);
    }
}
