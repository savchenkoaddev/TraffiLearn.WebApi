using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.ServiceCenters.Errors
{
    public static class ServiceCenterErrors
    {
        public static readonly Error NotFound =
            Error.NotFound(
                code: "ServiceCenter.NotFound",
                description: "Service center has not been found.");
    }
}
