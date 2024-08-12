using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Abstractions.Identity
{
    public interface IIdentityService<TIdentityUser>
    {
        Task CreateAsync(TIdentityUser identityUser, string password);

        Task DeleteAsync(TIdentityUser identityUser);

        Task<TIdentityUser?> GetByEmailAsync(Email email);

        Task AddToRoleAsync(TIdentityUser identityUser, string roleName);

        Task RemoveFromRoleAsync(TIdentityUser identityUser, string roleName);
    }
}
