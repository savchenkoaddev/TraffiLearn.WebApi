using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Users.Errors
{
    public static class UsernameErrors
    {
        public static readonly Error Empty =
            Error.Validation(
                code: "Username.Empty",
                description: "Username cannot be empty.");

        public static Error TooLong(int maxLength) =>
            Error.Validation(
                code: "Username.TooLong",
                description: $"Username exceeds {maxLength} characters.");
    }
}
