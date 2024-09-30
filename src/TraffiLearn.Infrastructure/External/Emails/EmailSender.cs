using Microsoft.AspNetCore.Identity.UI.Services;

namespace TraffiLearn.Infrastructure.External.Emails
{
    internal sealed class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(
            string email, 
            string subject, 
            string htmlMessage)
        {
            throw new NotImplementedException();
        }
    }
}
