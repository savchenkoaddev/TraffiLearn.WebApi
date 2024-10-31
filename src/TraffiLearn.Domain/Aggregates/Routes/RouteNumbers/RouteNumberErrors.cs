using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Routes.RouteNumbers
{
    public static class RouteNumberErrors
    {
        public static Error TooSmall(int minValue) =>
            Error.Validation(
                code: "RouteNumber.TooSmall",
                description: $"Route number must be greater or equal to {minValue}");
    }
}
