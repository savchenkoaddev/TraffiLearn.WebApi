﻿using TraffiLearn.Application.Abstractions.Emails;

namespace TraffiLearn.Infrastructure.Services.Emails
{
    internal sealed class EmailLetterCreator : IEmailLetterCreator
    {
        public Letter CreateChangeEmailLetter(string confirmChangeEmailLink)
        {
            return new Letter(
                Subject: CreateChangeEmailLetterSubject(),
                HtmlBody: CreateChangeEmailLetterBody(confirmChangeEmailLink));
        }

        private static string CreateChangeEmailLetterSubject() =>
            "Confirm Your Email Change Request";

        private static string CreateChangeEmailLetterBody(string confirmationLink)
        {
            return $@"
                <html>
                <body style='font-family: Arial, sans-serif; line-height: 1.5;'>
                    <h1 style='color: #4CAF50;'>Confirm Your Email Change</h1>
                    <p>You recently requested to change your email address for your TraffiLearn account. Please confirm this request by clicking the link below:</p>
                    <a href=""{confirmationLink}"" style='background-color: #4CAF50; color: white; padding: 10px 15px; text-decoration: none; border-radius: 5px;'>Confirm Email Change</a>
                    <p>If you did not make this request, please ignore this email.</p>
                    <i>Best regards,<br>The TraffiLearn Team</i>
                </body>
                </html>";
        }

        public Letter CreateEmailConfirmationLetter(string confirmationLink)
        {
            return new Letter(
                Subject: CreateEmailConfirmationLetterSubject(),
                HtmlBody: CreateEmailConfirmationLetterBody(confirmationLink));
        }

        private static string CreateEmailConfirmationLetterSubject() =>
          "Welcome to TraffiLearn! Please Confirm Your Email Address";

        private static string CreateEmailConfirmationLetterBody(string confirmationLink)
        {
            return $@"
                <html>
                <body style='font-family: Arial, sans-serif; line-height: 1.5;'>
                    <h1 style='color: #4CAF50;'>Welcome to TraffiLearn!</h1>
                    <p>Thank you for registering with us! To activate your account and start using our services, please confirm your email address by clicking the link below:</p>
                    <a href=""{confirmationLink}"" style='background-color: #4CAF50; color: white; padding: 10px 15px; text-decoration: none; border-radius: 5px;'>Confirm Your Registration</a>
                    <p>If you did not create an account, please ignore this email.</p>
                    <i>Best regards,<br>The TraffiLearn Team</i>
                </body>
                </html>";
        }

        public Letter CreateRecoverPasswordLetter(string resetPasswordLink)
        {
            return new Letter(
                Subject: CreateRecoverPasswordLetterSubject(),
                HtmlBody: CreateRecoverPasswordLetterBody(resetPasswordLink));
        }

        private static string CreateRecoverPasswordLetterSubject() =>
            "Reset Your TraffiLearn Password";

        private static string CreateRecoverPasswordLetterBody(string resetPasswordLink)
        {
            return $@"
                <html>
                <body style='font-family: Arial, sans-serif; line-height: 1.5;'>
                    <h1 style='color: #4CAF50;'>Reset Your Password</h1>
                    <p>You recently requested to reset your password for your TraffiLearn account. Please click the link below to reset your password:</p>
                    <a href=""{resetPasswordLink}"" style='background-color: #4CAF50; color: white; padding: 10px 15px; text-decoration: none; border-radius: 5px;'>Reset Password</a>  
                    <p>If you did not make this request, please ignore this email.</p>
                    <strong><span style='color: #ff0000'>Remember!</span> Don't share this email with anyone.</strong>
                    <br /><br />
                    <i>Best regards,<br>The TraffiLearn Team</i>
                </body>
                </html>";
        }
    }
}
