using System.Text;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Domain.Users.Usernames;

namespace TraffiLearn.Application.Services
{
    internal sealed class UsernameGenerator : IUsernameGenerator
    {
        private static readonly string Characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly Random Random = new();

        public Username Generate()
        {
            var username = new StringBuilder("user");

            for (int i = 0; i < Username.MaxLength - 1; i++)
            {
                char randomChar = Characters[Random.Next(Characters.Length)];
                username.Append(randomChar);
            }

            return Username.Create(username.ToString()).Value;
        }
    }
}
