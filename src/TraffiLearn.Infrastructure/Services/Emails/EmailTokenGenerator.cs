using Microsoft.AspNetCore.Identity;
using TraffiLearn.Application.Abstractions.Emails;
using TraffiLearn.Application.Users.Identity;

namespace TraffiLearn.Infrastructure.Services.Emails
{
    internal sealed class EmailTokenGenerator : IEmailTokenGenerator
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public EmailTokenGenerator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public Task<string> GenerateChangeEmailTokenAsync(
            ApplicationUser identityUser, 
            string newEmail)
        {
            return _userManager.GenerateChangeEmailTokenAsync(
                identityUser, newEmail);
        }

        public Task<string> GenerateConfirmationTokenAsync(
            ApplicationUser identityUser)
        {
            return _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
        }
    }
}
