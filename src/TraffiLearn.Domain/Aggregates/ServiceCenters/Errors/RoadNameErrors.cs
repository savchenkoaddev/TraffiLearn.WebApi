using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.ServiceCenters.Errors
{
    public static class RoadNameErrors
    {
        public static readonly Error Empty =
            Error.Validation(
                code: "RoadName.Empty",
                description: "Road name cannot be empty.");

        public static Error TooLong(int allowedLength) =>
            Error.Validation(
                code: "RoadName.TooLong",
                description: $"Road name must not exceed {allowedLength} characters.");
    }
}
