using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TraffiLearn.IntegrationTests.Auth
{
    internal static class AuthConstants
    {
        public const string Scheme =
            JwtBearerDefaults.AuthenticationScheme;
    }
}
