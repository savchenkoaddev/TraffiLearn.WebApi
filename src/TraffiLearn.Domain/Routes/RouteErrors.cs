using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.Routes
{
    public static class RouteErrors
    {
        public static readonly Error NotFound =
            Error.NotFound(
                code: "Route.NotFound",
                description: "Route has not been found.");
    }
}
