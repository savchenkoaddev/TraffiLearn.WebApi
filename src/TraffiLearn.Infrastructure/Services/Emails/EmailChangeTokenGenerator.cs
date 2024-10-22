using Microsoft.AspNetCore.Identity;
using TraffiLearn.Application.Abstractions.Emails;
using TraffiLearn.Application.Users.Identity;

namespace TraffiLearn.Infrastructure.Services.Emails
{
    internal sealed class EmailChangeTokenGenerator 
        : IEmailChangeTokenGenerator
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public EmailChangeTokenGenerator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public Task<string> Generate(
            ApplicationUser identityUser, 
            string newEmail)
        {
            return _userManager.GenerateChangeEmailTokenAsync(identityUser, newEmail);
        }
    }
}
