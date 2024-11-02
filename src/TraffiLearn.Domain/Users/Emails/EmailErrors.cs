using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.Users.Emails
{
    public static class EmailErrors
    {
        public static readonly Error Empty =
            Error.Validation(
                code: "Email.Empty",
                description: "Email cannot be empty.");

        public static readonly Error InvalidFormat =
            Error.Validation(
                code: "Email.InvalidFormat",
                description: "Email is in invalid format.");

        public static Error TooLong(int maxLength) =>
            Error.Validation(
                code: "Email.TooLong",
                description: $"Email exceeds {maxLength} characters.");
    }
}
