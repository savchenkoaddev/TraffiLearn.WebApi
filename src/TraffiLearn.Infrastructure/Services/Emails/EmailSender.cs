using FluentEmail.Core;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Emails;
using TraffiLearn.Infrastructure.Exceptions;

namespace TraffiLearn.Infrastructure.Services.Emails
{
    internal sealed class EmailSender : IEmailSender
    {
        private readonly IFluentEmailFactory _emailFactory;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(
            IFluentEmailFactory emailFactory,
            ILogger<EmailSender> logger)
        {
            _emailFactory = emailFactory;
            _logger = logger;
        }

        public async Task SendEmailAsync(
            string recipientEmail,
            string subject,
            string htmlBody)
        {
            var emailMessage = _emailFactory
                .Create()
                .To(recipientEmail)
                .Subject(subject)
                .Body(htmlBody, isHtml: true);

            var result = await emailMessage.SendAsync();

            if (!result.Successful)
            {
                _logger.LogError($"Email sending failed: {string.Join(", ", result.ErrorMessages)}");

                throw new EmailSendingFailedException();
            }
        }
    }
}
