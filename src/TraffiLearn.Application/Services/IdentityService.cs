using Microsoft.AspNetCore.Identity;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Identity;

namespace TraffiLearn.Application.Services
{
    public sealed class IdentityService : IIdentityService<ApplicationUser>
    {
        public Task CreateAsync(ApplicationUser identityUser)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(ApplicationUser identityUser)
        {
            throw new NotImplementedException();
        }

        public Task AddToRoleAsync(ApplicationUser user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(ApplicationUser user, string roleName)
        {
            throw new NotImplementedException();
        }
    }
}
