using TraffiLearn.Application.Abstractions.Emails;
using TraffiLearn.Application.UseCases.Users.Identity;
using TraffiLearn.Domain.SubscriptionPlans;

namespace TraffiLearn.Application.Services
{
    internal sealed class EmailService : IEmailService
    {
        private readonly IEmailPublisher _emailPublisher;
        private readonly IEmailTokenGenerator _emailTokenGenerator;
        private readonly IEmailLinkGenerator _emailLinkGenerator;
        private readonly IEmailLetterCreator _emailLetterCreator;

        public EmailService(
            IEmailPublisher emailSender,
            IEmailTokenGenerator emailTokenGenerator,
            IEmailLinkGenerator emailLinkGenerator,
            IEmailLetterCreator emailLetterCreator)
        {
            _emailPublisher = emailSender;
            _emailTokenGenerator = emailTokenGenerator;
            _emailLinkGenerator = emailLinkGenerator;
            _emailLetterCreator = emailLetterCreator;
        }

        public async Task PublishConfirmationEmailAsync(
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

            await _emailPublisher.PublishEmailMessageAsync(
                recipientEmail,
                letter.Subject,
                letter.HtmlBody);
        }

        public async Task PublishChangeEmailMessageAsync(
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

            await _emailPublisher.PublishEmailMessageAsync(
                recipientEmail: newEmail,
                subject: letter.Subject,
                htmlBody: letter.HtmlBody);
        }

        public async Task PublishRecoverPasswordEmail(
            string recipientEmail,
            string userId,
            ApplicationUser identityUser)
        {
            var token = await _emailTokenGenerator
                .GenerateRecoverPasswordTokenAsync(identityUser);

            var escapedToken = Uri.EscapeDataString(token);

            var link = _emailLinkGenerator
                .GenerateRecoverPasswordLink(userId, escapedToken);

            Letter letter = _emailLetterCreator
                .CreateRecoverPasswordLetter(link);

            await _emailPublisher.PublishEmailMessageAsync(
                recipientEmail,
                letter.Subject,
                letter.HtmlBody);
        }

        public async Task PublishPlanExpiryReminderEmailAsync(
            string recipientEmail,
            DateTime planExpiresOn,
            int days)
        {
            Letter letter = _emailLetterCreator
                .CreatePlanExpiryReminderLetter(days, planExpiresOn);

            await _emailPublisher.PublishEmailMessageAsync(
                recipientEmail,
                letter.Subject,
                letter.HtmlBody);
        }

        public async Task PublishPlanRenewedEmailAsync(
            string recipientEmail,
            DateTime planExpiresOn)
        {
            Letter letter = _emailLetterCreator
                .CreatePlanRenewedLetter(planExpiresOn);
                
            await _emailPublisher.PublishEmailMessageAsync(
                recipientEmail,
                letter.Subject,
                letter.HtmlBody);
        }
        
        public async Task PublishPlanCancelationEmailAsync(string recipientEmail)
        {
            Letter letter = _emailLetterCreator
                .CreatePlanCancelationLetter();

            await _emailPublisher.PublishEmailMessageAsync(
                recipientEmail,
                letter.Subject,
                letter.HtmlBody);
        }
    }
}
