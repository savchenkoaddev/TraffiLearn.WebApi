namespace TraffiLearn.Application.Abstractions.Identity
{
    public interface IRoleService<TRole>
    {
        Task CreateAsync(TRole role);

        Task<bool> ExistsAsync(string roleName);
    }
}
