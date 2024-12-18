﻿namespace TraffiLearn.Application.Abstractions.Emails
{
    public interface IEmailLetterCreator
    {
        Letter CreateEmailConfirmationLetter(string confirmationLink);

        Letter CreateChangeEmailLetter(string confirmChangeEmailLink);

        Letter CreateRecoverPasswordLetter(string recoverPasswordLink);

        Letter CreatePlanExpiryReminderLetter(
            int days,
            DateTime planExpiresOn);

        Letter CreatePlanCancelationLetter();
    }
}
