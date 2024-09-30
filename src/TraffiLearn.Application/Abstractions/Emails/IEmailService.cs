namespace TraffiLearn.Application.Abstractions.Emails
{
    public interface IEmailService
    {
        Task SendConfirmationEmail(
            string recipientEmail,
            string confirmationLink);
    }
}
