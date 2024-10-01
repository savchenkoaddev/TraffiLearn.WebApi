using Microsoft.AspNetCore.Identity;
using TraffiLearn.Application.Abstractions.Emails;
using TraffiLearn.Application.Users.Identity;

namespace TraffiLearn.Infrastructure.Services.Emails
{
    internal sealed class ConfirmationTokenGenerator : IConfirmationTokenGenerator
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ConfirmationTokenGenerator(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public Task<string> Generate(ApplicationUser identityUser)
        {
            return _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
        }
    }
}
