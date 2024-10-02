using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Regions.Errors
{
    public static class RegionNameErrors
    {
        public static readonly Error Empty =
            Error.Validation(
                code: "RegionName.Empty",
                description: "Region name cannot be empty.");

        public static Error TooLong(int allowedLength) =>
            Error.Validation(
                code: "RegionName.TooLong",
                description: $"Region name must not exceed {allowedLength} characters.");
    }
}
