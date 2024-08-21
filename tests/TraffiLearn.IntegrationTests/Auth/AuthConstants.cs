using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TraffiLearn.IntegrationTests.Auth
{
    internal static class AuthConstants
    {
        public const string Scheme =
            JwtBearerDefaults.AuthenticationScheme;

        public const string Username = "username";

        public const string Email = "email@email.com";

        public const string Password = "Strong123!.";

        public const string AdminUsername = "admin";

        public const string AdminEmail = "admin@email.com";

        public const string OwnerEmail = "owner@owner.com";

        public const string OwnerUsername = "owner";
    }
}
