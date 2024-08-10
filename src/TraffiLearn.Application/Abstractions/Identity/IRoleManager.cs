namespace TraffiLearn.Application.Abstractions.Identity
{
    public interface IRoleManager<TRole>
    {
        Task CreateRole(TRole role);

        Task<bool> RoleExists(string roleName);
    }
}
