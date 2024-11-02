using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.ServiceCenters.BuildingNumbers
{
    public static class BuildingNumberErrors
    {
        public static readonly Error Empty =
            Error.Validation(
                code: "BuildingNumber.Empty",
                description: "Building number name cannot be empty.");

        public static Error TooLong(int allowedLength) =>
            Error.Validation(
                code: "BuildingNumber.TooLong",
                description: $"Building number must not exceed {allowedLength} characters.");
    }
}
