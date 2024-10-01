using TraffiLearn.Application.Users.Identity;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;

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
