namespace TraffiLearn.Infrastructure.Exceptions
{
    public sealed class EmailSendingFailedException : Exception
    {
        public EmailSendingFailedException()
            : base("Failed to send email due to some reasons.")
        { }
    }
}
