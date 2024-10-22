using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Abstractions.Identity
{
    public interface IGoogleAuthService
    {
        Task<Result<Email>> ValidateIdTokenAsync(string token);
    }
}
