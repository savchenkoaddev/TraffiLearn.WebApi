namespace TraffiLearn.Application.Abstractions.Identity
{
    public interface IRoleManager<TUser, TRole>
    {
        Task CreateRole(TRole role);

        Task<TRole> GetUserRole(TUser user);
    }
}
