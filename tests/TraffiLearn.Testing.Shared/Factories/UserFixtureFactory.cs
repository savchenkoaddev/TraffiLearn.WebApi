using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;

namespace TraffiLearn.Testing.Shared.Factories
{
    public static class UserFixtureFactory
    {
        public static User CreateUser()
        {
            return User.Create(
                new UserId(Guid.NewGuid()),
                CreateEmail(),
                CreateUsername(),
                CreateRole()).Value;
        }

        public static Username CreateUsername()
        {
            return Username.Create("Username").Value;
        }

        public static Email CreateEmail()
        {
            return Email.Create(
                "email@email.com").Value;
        }

        public static Role CreateRole()
        {
            return Role.RegularUser;
        }
    }
}
