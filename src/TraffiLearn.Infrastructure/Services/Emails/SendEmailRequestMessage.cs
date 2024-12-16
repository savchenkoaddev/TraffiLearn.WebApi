namespace TraffiLearn.Application.Abstractions.Emails
{
    internal sealed record SendEmailRequestMessage(
        string RecipientEmail,
        string Subject,
        string HtmlBody);
}
