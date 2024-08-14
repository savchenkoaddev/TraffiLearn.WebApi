using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Enums;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.DomainTests.Factories
{
    internal static class UserFixtureFactory
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
