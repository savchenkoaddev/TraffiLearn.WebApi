namespace TraffiLearn.Application.Abstractions.Emails
{
    public interface IEmailPublisher
    {
        Task PublishEmailMessageAsync(
            string recipientEmail,
            string subject,
            string htmlBody);
    }
}
