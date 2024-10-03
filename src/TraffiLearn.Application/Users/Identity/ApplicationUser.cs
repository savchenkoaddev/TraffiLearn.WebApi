using Microsoft.AspNetCore.Identity;

namespace TraffiLearn.Application.Users.Identity
{
    public sealed class ApplicationUser : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpirationTime { get; set; }
    }
}
