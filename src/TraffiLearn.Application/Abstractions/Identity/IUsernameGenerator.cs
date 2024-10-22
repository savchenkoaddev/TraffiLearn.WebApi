using TraffiLearn.Domain.Aggregates.Users.ValueObjects;

namespace TraffiLearn.Application.Abstractions.Identity
{
    public interface IUsernameGenerator
    {
        Username Generate();
    }
}
