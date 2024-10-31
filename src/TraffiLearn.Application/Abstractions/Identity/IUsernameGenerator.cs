using TraffiLearn.Domain.Aggregates.Users.Usernames;

namespace TraffiLearn.Application.Abstractions.Identity
{
    public interface IUsernameGenerator
    {
        Username Generate();
    }
}
