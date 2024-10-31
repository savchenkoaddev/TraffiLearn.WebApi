using TraffiLearn.Domain.Aggregates.Users.Emails;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Abstractions.Identity
{
    public interface IGoogleAuthService
    {
        Task<Result<Email>> ValidateIdTokenAsync(string token);
    }
}
