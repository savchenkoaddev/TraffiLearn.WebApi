using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Errors.Users
{
    public static class UserErrors
    {
        public static readonly Error NotFound =
            Error.NotFound(
                code: "User.NotFound",
                description: "User has not been found.");

        public static readonly Error AlreadyRegistered =
            Error.Validation(
                code: "User.AlreadyRegistered",
                description: "The same user has already been registered.");
    }
}
