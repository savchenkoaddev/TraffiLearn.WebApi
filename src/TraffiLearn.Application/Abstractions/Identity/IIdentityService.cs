namespace TraffiLearn.Application.Abstractions.Identity
{
    public interface IIdentityService<TIdentityUser>
    {
        Task CreateAsync(TIdentityUser identityUser, string password);

        Task DeleteAsync(TIdentityUser identityUser);

        Task AddToRoleAsync(TIdentityUser user, string roleName);

        Task RemoveFromRoleAsync(TIdentityUser user, string roleName);
    }
}
