using TraffiLearn.Application.Users.Identity;

namespace TraffiLearn.Application.Abstractions.Emails
{
    public interface IEmailService
    {
        Task SendConfirmationEmail(
            string recipientEmail,
            string userId,
            ApplicationUser applicationUser);
    }
}
