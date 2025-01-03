﻿using Microsoft.AspNetCore.Identity;

namespace TraffiLearn.Application.UseCases.Users.Identity
{
    public sealed class ApplicationUser : IdentityUser
    {
        public string? RefreshTokenHash { get; set; }

        public DateTime RefreshTokenExpirationTime { get; set; }
    }
}
