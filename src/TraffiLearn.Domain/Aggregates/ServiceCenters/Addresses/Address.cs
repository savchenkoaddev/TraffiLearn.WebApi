using TraffiLearn.Domain.Aggregates.ServiceCenters.BuildingNumbers;
using TraffiLearn.Domain.Aggregates.ServiceCenters.LocationNames;
using TraffiLearn.Domain.Aggregates.ServiceCenters.RoadNames;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.ServiceCenters.Addresses
{
    public sealed class Address : ValueObject
    {
        private Address(
            LocationName locationName,
            RoadName roadName,
            BuildingNumber buildingNumber)
        {
            LocationName = locationName;
            RoadName = roadName;
            BuildingNumber = buildingNumber;
        }

        public LocationName LocationName { get; }

        public RoadName RoadName { get; }

        public BuildingNumber BuildingNumber { get; }

        public static Result<Address> Create(
            LocationName locationName,
            RoadName roadName,
            BuildingNumber buildingNumber)
        {
            if (locationName is null)
            {
                return Result.Failure<Address>(
                    AddressErrors.LocationNameEmpty);
            }

            if (roadName is null)
            {
                return Result.Failure<Address>(
                    AddressErrors.RoadNameEmpty);
            }

            if (buildingNumber is null)
            {
                return Result.Failure<Address>(
                    AddressErrors.BuildingNumberEmpty);
            }

            return new Address(locationName, roadName, buildingNumber);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return LocationName;
            yield return RoadName;
            yield return BuildingNumber;
        }
    }
}
