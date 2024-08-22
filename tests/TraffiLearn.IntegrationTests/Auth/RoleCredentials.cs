namespace TraffiLearn.IntegrationTests.Auth
{
    public class RoleCredentials
    {
        public RoleCredentials(
            string email,
            string username,
            string password)
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Password = password ?? throw new ArgumentNullException(nameof(password));
        }

        public string Email { get; }

        public string Username { get; }

        public string Password { get; }
    }
}
