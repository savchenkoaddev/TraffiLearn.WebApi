namespace TraffiLearn.IntegrationTests.Auth
{
    internal static class AuthTestCredentials
    {
        public static readonly RoleCredentials RegularUser =
            new RoleCredentials(
                email: "email@email.com",
                username: "regular-user",
                password: CommonPassword);

        public static readonly RoleCredentials Admin =
            new RoleCredentials(
                email: "admin@admin.com",
                username: "admin",
                password: CommonPassword);

        public static readonly RoleCredentials Owner =
            new RoleCredentials(
                email: "test-owner@owner.com",
                username: "test-owner",
                password: CommonPassword);

        private const string CommonPassword = "Strong123!.";
    }
}
