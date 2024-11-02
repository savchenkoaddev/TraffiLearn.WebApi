using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.Regions
{
    public static class RegionErrors
    {
        public static readonly Error NotFound =
            Error.NotFound(
                code: "Region.NotFound",
                description: "Region has not been found.");
    }
}
