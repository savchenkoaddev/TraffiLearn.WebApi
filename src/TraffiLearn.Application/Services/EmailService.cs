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
            var encodedToken = Uri.EscapeDataString(token);

            var link = _confirmationLinkGenerator.Generate(userId, encodedToken);

            string subject = CreateEmailConfirmationSubject();
            string htmlBody = CreateEmailConfirmationBody(link);

            Console.WriteLine(subject);
            Console.WriteLine(htmlBody);

            await _emailSender.SendEmailAsync(
                recipientEmail,
                subject,
                htmlBody);
        }

        private static string CreateEmailConfirmationSubject() =>
            "Confirm your registration";

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
