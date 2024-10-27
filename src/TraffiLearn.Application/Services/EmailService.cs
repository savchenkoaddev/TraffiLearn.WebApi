using TraffiLearn.Application.Abstractions.Emails;
using TraffiLearn.Application.Users.Identity;

namespace TraffiLearn.Application.Services
{
    internal sealed class EmailService : IEmailService
    {
        private readonly IEmailSender _emailSender;
        private readonly IEmailTokenGenerator _emailTokenGenerator;
        private readonly IEmailLinkGenerator _emailLinkGenerator;
        private readonly IEmailLetterCreator _emailLetterCreator;

        public EmailService(
            IEmailSender emailSender,
            IEmailTokenGenerator emailTokenGenerator,
            IEmailLinkGenerator emailLinkGenerator,
            IEmailLetterCreator emailLetterCreator)
        {
            _emailSender = emailSender;
            _emailTokenGenerator = emailTokenGenerator;
            _emailLinkGenerator = emailLinkGenerator;
            _emailLetterCreator = emailLetterCreator;
        }

        public async Task SendConfirmationEmailAsync(
            string recipientEmail,
            string userId,
            ApplicationUser applicationUser)
        {
            var token = await _emailTokenGenerator
                .GenerateConfirmationTokenAsync(applicationUser);

            var escapedToken = Uri.EscapeDataString(token);

            var link = _emailLinkGenerator.GenerateConfirmationLink(userId, escapedToken);

            Letter letter = _emailLetterCreator
                .CreateEmailConfirmationLetter(link);

            await _emailSender.SendEmailAsync(
                recipientEmail,
                letter.Subject,
                letter.HtmlBody);
        }

        public async Task SendChangeEmailMessageAsync(
            string newEmail, 
            string userId,
            ApplicationUser identityUser)
        {
            var token = await _emailTokenGenerator
                .GenerateChangeEmailTokenAsync(
                    identityUser, newEmail);

            var escapedToken = Uri.EscapeDataString(token);

            var link = _emailLinkGenerator
                .GenerateConfirmChangeEmailLink(
                    userId, newEmail, escapedToken);

            Letter letter = _emailLetterCreator
                .CreateChangeEmailLetter(link);

            await _emailSender.SendEmailAsync(
                recipientEmail: newEmail,
                subject: letter.Subject,
                htmlBody: letter.HtmlBody);
        }
    }
}
