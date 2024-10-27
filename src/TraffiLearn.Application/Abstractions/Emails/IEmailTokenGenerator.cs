using TraffiLearn.Application.Users.Identity;

namespace TraffiLearn.Application.Abstractions.Emails
{
    public interface IEmailTokenGenerator
    {
        Task<string> GenerateConfirmationTokenAsync(ApplicationUser identityUser);

        Task<string> GenerateChangeEmailTokenAsync(
            ApplicationUser identityUser,
            string newEmail);

        Task<string> GenerateRecoverPasswordTokenAsync(
            ApplicationUser identityUser);
    }
}
