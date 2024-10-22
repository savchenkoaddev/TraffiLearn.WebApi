using TraffiLearn.Application.Users.Identity;

namespace TraffiLearn.Application.Abstractions.Emails
{
    public interface IEmailChangeTokenGenerator
    {
        Task<string> Generate(
            ApplicationUser identityUser, 
            string newEmail);
    }
}
