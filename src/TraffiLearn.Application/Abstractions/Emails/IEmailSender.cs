namespace TraffiLearn.Application.Abstractions.Emails
{
    public interface IEmailSender
    {
        Task SendEmailAsync(
            string recipientEmail,
            string subject,
            string body);
    }
}
