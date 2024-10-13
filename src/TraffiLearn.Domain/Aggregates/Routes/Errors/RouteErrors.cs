using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Routes.Errors
{
    public static class RouteErrors
    {
        public static readonly Error NotFound =
            Error.NotFound(
                code: "Route.NotFound",
                description: "Route has not been found.");
    }
}
