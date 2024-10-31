using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.Emails;
using TraffiLearn.Domain.Aggregates.Users.Roles;
using TraffiLearn.Domain.Aggregates.Users.Usernames;

namespace TraffiLearn.Testing.Shared.Factories
{
    public static class UserFixtureFactory
    {
        public static User CreateUser(string email = "email@email.com",
            string username = "Username")
        {
            return User.Create(
                new UserId(Guid.NewGuid()),
                CreateEmail(email),
                CreateUsername(username),
                CreateRole()).Value;
        }

        public static Username CreateUsername(string username = "Username")
        {
            return Username.Create(username).Value;
        }

        public static Email CreateEmail(string email = "email@email.com")
        {
            return Email.Create(email).Value;
        }

        public static Role CreateRole()
        {
            return Role.RegularUser;
        }

        public static string CreateValidPassword() => "Strong123!.";
    }
}
