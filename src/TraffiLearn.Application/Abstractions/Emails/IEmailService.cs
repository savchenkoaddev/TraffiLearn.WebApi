﻿using TraffiLearn.Application.UseCases.Users.Identity;

namespace TraffiLearn.Application.Abstractions.Emails
{
    public interface IEmailService
    {
        Task SendConfirmationEmailAsync(
            string recipientEmail,
            string userId,
            ApplicationUser identityUser);

        Task SendChangeEmailMessageAsync(
            string newEmail,
            string userId,
            ApplicationUser identityUser);

        Task SendRecoverPasswordEmail(
            string recipientEmail,
            string userId,
            ApplicationUser identityUser);
    }
}
