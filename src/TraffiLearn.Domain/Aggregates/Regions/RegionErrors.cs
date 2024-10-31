using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Regions
{
    public static class RegionErrors
    {
        public static readonly Error NotFound =
            Error.NotFound(
                code: "Region.NotFound",
                description: "Region has not been found.");
    }
}
