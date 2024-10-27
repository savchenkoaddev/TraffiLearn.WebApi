namespace TraffiLearn.Application.Abstractions.Emails
{
    public interface IEmailLetterCreator
    {
        Letter CreateEmailConfirmationLetter(string confirmationLink);

        Letter CreateChangeEmailLetter(string confirmChangeEmailLink);
    }
}
