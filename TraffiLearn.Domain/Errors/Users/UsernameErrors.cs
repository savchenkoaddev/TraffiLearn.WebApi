using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Errors.Users
{
    public static class UsernameErrors
    {
        public static readonly Error Empty =
            Error.Validation(
                code: "Username.Empty",
                description: "Username cannot be empty.");
    }
}
