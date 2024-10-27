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

        Task<Result> ChangeEmailAsync(
            TIdentityUser identityUser,
            string newEmail,
            string token);

        Task AddToRoleAsync(TIdentityUser identityUser, string roleName);

        Task RemoveFromRoleAsync(TIdentityUser identityUser, string roleName);

        Task<Result> LoginAsync(TIdentityUser identityUser, string password);

        Task PopulateRefreshTokenAsync(TIdentityUser identityUser, string refreshToken);

        Result ValidateRefreshToken(TIdentityUser user);

        Task<Result<TIdentityUser>> GetByRefreshTokenAsync(string refreshToken);

        Task<Result> ResetPasswordAsync(
            TIdentityUser identityUser,
            string newPassword,
            string token);
    }
}
