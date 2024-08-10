namespace TraffiLearn.Application.Abstractions.Identity
{
    public interface IIdentityService<TIdentityUser, TRole>
    {
        Task CreateAsync(TIdentityUser identityUser);

        Task DeleteAsync(TIdentityUser identityUser);

        Task AddToRoleAsync(TIdentityUser user, TRole role);

        Task RemoveFromRoleAsync(TIdentityUser user, TRole role);
    }
}
