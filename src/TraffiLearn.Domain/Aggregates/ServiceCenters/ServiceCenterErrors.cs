using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.ServiceCenters
{
    public static class ServiceCenterErrors
    {
        public static readonly Error NotFound =
            Error.NotFound(
                code: "ServiceCenter.NotFound",
                description: "Service center has not been found.");

        public static readonly Error RouteNotFound =
            Error.NotFound(
                code: "ServiceCenter.RouteNotFound",
                description: "The service center does not contain the route.");

        public static readonly Error RouteAlreadyAdded =
            Error.Validation(
                code: "ServiceCenter.RouteAlreadyAdded",
                description: "The service center already contains the route.");
    }
}
