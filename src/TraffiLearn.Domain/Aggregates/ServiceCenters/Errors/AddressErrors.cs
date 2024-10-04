using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.ServiceCenters.Errors
{
    public static class AddressErrors
    {
        public static readonly Error LocationNameEmpty =
            Error.Validation(
                code: "Address.LocationNameEmpty",
                description: "Location name must not be empty.");

        public static readonly Error RoadNameEmpty =
            Error.Validation(
                code: "Address.RoadNameEmpty",
                description: "Road name must not be empty.");

        public static readonly Error BuildingNumberEmpty =
            Error.Validation(
                code: "Address.BuildingNumberEmpty",
                description: "Building number must not be empty.");
    }
}
