using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.ServiceCenters.LocationNames
{
    public sealed class LocationNameErrors
    {
        public static readonly Error Empty =
            Error.Validation(
                code: "LocationName.Empty",
                description: "Location name cannot be empty.");

        public static Error TooLong(int allowedLength) =>
            Error.Validation(
                code: "LocationName.TooLong",
                description: $"Location name must not exceed {allowedLength} characters.");
    }
}
