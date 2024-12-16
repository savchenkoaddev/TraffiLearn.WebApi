using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Emails;
using TraffiLearn.Application.Abstractions.EventBus;

namespace TraffiLearn.Infrastructure.Services.Emails
{
    internal sealed class EmailSender : IEmailPublisher
    {
        private readonly IEventBus _eventBus;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(
            IEventBus eventBus,
            ILogger<EmailSender> logger)
        {
            _eventBus = eventBus;
            _logger = logger;
        }

        public async Task PublishEmailMessageAsync(
            string recipientEmail,
            string subject,
            string htmlBody)
        {
            var publishMessage = new SendEmailRequestMessage(
                RecipientEmail: recipientEmail,
                Subject: subject,
                HtmlBody: htmlBody);

            await _eventBus.PublishAsync(publishMessage);
        }
    }
}
