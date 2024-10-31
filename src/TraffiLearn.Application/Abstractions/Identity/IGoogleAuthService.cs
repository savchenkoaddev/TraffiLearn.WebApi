using TraffiLearn.Domain.Users.Emails;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Abstractions.Identity
{
    public interface IGoogleAuthService
    {
        Task<Result<Email>> ValidateIdTokenAsync(string token);
    }
}
