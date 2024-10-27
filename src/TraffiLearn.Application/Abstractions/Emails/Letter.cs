namespace TraffiLearn.Application.Abstractions.Emails
{
    public sealed record Letter(
        string Subject,
        string HtmlBody);
}
