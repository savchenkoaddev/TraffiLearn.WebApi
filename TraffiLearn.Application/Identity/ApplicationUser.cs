using Microsoft.AspNetCore.Identity;

namespace TraffiLearn.Application.Identity
{
    public sealed class ApplicationUser : IdentityUser
    {
        public string Username { get; }
    }
}
