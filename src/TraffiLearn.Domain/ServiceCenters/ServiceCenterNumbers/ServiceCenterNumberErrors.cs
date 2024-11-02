using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.ServiceCenters.ServiceCenterNumbers
{
    public static class ServiceCenterNumberErrors
    {
        public static readonly Error Empty =
            Error.Validation(
                code: "ServiceCenterNumber.Empty",
                description: "Service center number cannot be empty.");

        public static readonly Error NotNumber =
            Error.Validation(
                code: "ServiceCenterNumber.NotNumber",
                description: "Service center number must be a number.");

        public static Error TooLong(int allowedLength) =>
            Error.Validation(
                code: "ServiceCenterNumber.TooLong",
                description: $"Service center number name must not exceed {allowedLength} characters.");
    }
}
