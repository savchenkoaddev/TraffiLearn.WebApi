using TraffiLearn.Application.Abstractions.Emails;
using TraffiLearn.Application.Users.Identity;

namespace TraffiLearn.Application.Services
{
    internal sealed class EmailService : IEmailService
    {
        private readonly IEmailSender _emailSender;
        private readonly IConfirmationTokenGenerator _confirmationTokenGenerator;
        private readonly IEmailConfirmationLinkGenerator _confirmationLinkGenerator;

        public EmailService(
            IEmailSender emailSender,
            IConfirmationTokenGenerator confirmationTokenGenerator,
            IEmailConfirmationLinkGenerator confirmationLinkGenerator)
        {
            _emailSender = emailSender;
            _confirmationTokenGenerator = confirmationTokenGenerator;
            _confirmationLinkGenerator = confirmationLinkGenerator;
        }

        public async Task SendConfirmationEmail(
            string recipientEmail,
            string userId,
            ApplicationUser applicationUser)
        {
            var token = await _confirmationTokenGenerator.Generate(applicationUser);

            var escapedToken = Uri.EscapeDataString(token);

            var link = _confirmationLinkGenerator.Generate(userId, escapedToken);

            string subject = CreateEmailConfirmationSubject();
            string htmlBody = CreateEmailConfirmationBody(link);

            await _emailSender.SendEmailAsync(
                recipientEmail,
                subject,
                htmlBody);
        }

        private static string CreateEmailConfirmationSubject() =>
            "Welcome to TraffiLearn! Please Confirm Your Email Address";

        private static string CreateEmailConfirmationBody(string confirmationLink)
        {
            return $@"
                <html>
                <body style='font-family: Arial, sans-serif; line-height: 1.5;'>
                    <h1 style='color: #4CAF50;'>Welcome to TraffiLearn!</h1>
                    <p>Thank you for registering with us! To activate your account and start using our services, please confirm your email address by clicking the link below:</p>
                    <a href=""{confirmationLink}"" style='background-color: #4CAF50; color: white; padding: 10px 15px; text-decoration: none; border-radius: 5px;'>Confirm Your Registration</a>
                    <p>If you did not create an account, please ignore this email.</p>
                    <p>Best regards,<br>The TraffiLearn Team</p>
                </body>
                </html>";
        }
    }
}
