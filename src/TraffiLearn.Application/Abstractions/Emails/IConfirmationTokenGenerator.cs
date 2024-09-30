using TraffiLearn.Application.Users.Identity;

namespace TraffiLearn.Application.Abstractions.Emails
{
    public interface IConfirmationTokenGenerator
    {
        Task<string> Generate(ApplicationUser applicationUser);
    }
}
