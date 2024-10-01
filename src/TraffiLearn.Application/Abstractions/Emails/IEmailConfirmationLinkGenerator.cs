namespace TraffiLearn.Application.Abstractions.Emails
{
    public interface IEmailConfirmationLinkGenerator
    {
        string Generate(string userId, string token);
    }
}
