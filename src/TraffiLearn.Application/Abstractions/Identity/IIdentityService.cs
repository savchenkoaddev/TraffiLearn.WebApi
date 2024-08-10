namespace TraffiLearn.Application.Abstractions.Identity
{
    public interface IIdentityService<TIdentityUser>
    {
        Task CreateAsync(TIdentityUser identityUser, string password);

        Task DeleteAsync(TIdentityUser identityUser);

        Task AddToRoleAsync(TIdentityUser identityUser, string roleName);

        Task RemoveFromRoleAsync(TIdentityUser identityUser, string roleName);
    }
}
