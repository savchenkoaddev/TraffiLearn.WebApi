using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.ServiceCenters.Errors
{
    public static class ServiceCenterNumberErrors
    {
        public static readonly Error Empty =
            Error.Validation(
                code: "ServiceCenterNumber.Empty",
                description: "Service center number cannot be empty.");

        public static Error TooLong(int allowedLength) =>
            Error.Validation(
                code: "ServiceCenterNumber.TooLong",
                description: $"Service center number name must not exceed {allowedLength} characters.");
    }
}
