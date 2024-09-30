using TraffiLearn.Application.Abstractions.Emails;

namespace TraffiLearn.Application.Services
{
    internal sealed class EmailService : IEmailService
    {
        private readonly IEmailSender _emailSender;

        public EmailService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public Task SendConfirmationEmail(
            string recipientEmail, 
            string confirmationLink)
        {
            string subject = CreateEmailConfirmationSubject();
            string htmlBody = CreateEmailConfirmationBody(confirmationLink);

            return _emailSender.SendEmailAsync(
                recipientEmail,
                subject,
                htmlBody);
        }

        private static string CreateEmailConfirmationSubject()
        {
            return "Confirm your registration";
        }

        private static string CreateEmailConfirmationBody(string confirmationLink)
        {
            return $@"
                <h1>Welcome to TraffiLearn!</h1>
                <p>Please confirm your account by clicking the link below:</p>
                <a href=""{confirmationLink}"">Confirm your registration</a>
                <p>If you did not register, you can safely ignore this email.</p>";
        }
    }
}
