using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Abstractions.Identity
{
    public interface IIdentityService<TIdentityUser>
    {
        Task CreateAsync(TIdentityUser identityUser, string password);

        Task DeleteAsync(TIdentityUser identityUser);

        Task<TIdentityUser?> GetByEmailAsync(Email email);

        Task<Result> ConfirmEmailAsync(TIdentityUser identityUser, string token);

        Task AddToRoleAsync(TIdentityUser identityUser, string roleName);

        Task RemoveFromRoleAsync(TIdentityUser identityUser, string roleName);

        Task<Result> LoginAsync(TIdentityUser identityUser, string password);

        Task<Result> PopulateRefreshTokenAsync(TIdentityUser identityUser, string refreshToken);

        Task<Result> ValidateRefreshTokenAsync(TIdentityUser user, string refreshToken);

        Task<Result<TIdentityUser>> GetByAccessTokenAsync(string accessToken);
    }
}
