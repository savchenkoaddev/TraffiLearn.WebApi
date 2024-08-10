namespace TraffiLearn.Application.Abstractions.Identity
{
    public interface IRoleService<TRole>
    {
        Task CreateRole(TRole role);

        Task<bool> RoleExists(string roleName);
    }
}
