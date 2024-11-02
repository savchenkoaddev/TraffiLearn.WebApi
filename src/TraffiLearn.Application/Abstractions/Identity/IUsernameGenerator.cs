using TraffiLearn.Domain.Users.Usernames;

namespace TraffiLearn.Application.Abstractions.Identity
{
    public interface IUsernameGenerator
    {
        Username Generate();
    }
}
