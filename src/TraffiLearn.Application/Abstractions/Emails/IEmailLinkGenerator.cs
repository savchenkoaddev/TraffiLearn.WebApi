namespace TraffiLearn.Application.Abstractions.Emails
{
    public interface IEmailLinkGenerator
    {
        string GenerateConfirmationLink(string userId, string token);

        string GenerateConfirmChangeEmailLink(
            string userId, 
            string newEmail, 
            string token);
    }
}
