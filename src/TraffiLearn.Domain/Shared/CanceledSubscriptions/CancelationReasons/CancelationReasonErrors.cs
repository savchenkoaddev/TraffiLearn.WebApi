using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.Users.CancelationReasons
{
    public static class CancelationReasonErrors
    {
        public static readonly Error Empty =
            Error.Validation(
                code: "CancelationReason.Empty",
                description: "Cancelation reason cannot be empty.");

        public static Error TooLong(int maxLength) =>
            Error.Validation(
                code: "CancelationReason.TooLong",
                description: $"Cancelation reason exceeds {maxLength} characters.");
    }
}
