using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.ServiceCenters.Commands.Create;
using TraffiLearn.Domain.ServiceCenters;
using TraffiLearn.Domain.ServiceCenters.Addresses;
using TraffiLearn.Domain.ServiceCenters.BuildingNumbers;
using TraffiLearn.Domain.ServiceCenters.LocationNames;
using TraffiLearn.Domain.ServiceCenters.RoadNames;
using TraffiLearn.Domain.ServiceCenters.ServiceCenterNumbers;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.ServiceCenters.Mappers
{
    internal sealed class CreateServiceCenterCommandToEntityMapper
        : Mapper<CreateServiceCenterCommand, Result<ServiceCenter>>
    {
        public override Result<ServiceCenter> Map(CreateServiceCenterCommand source)
        {
            var serviceCenterNumberResult = ServiceCenterNumber.Create(
                source.ServiceCenterNumber);

            if (serviceCenterNumberResult.IsFailure)
            {
                return Result.Failure<ServiceCenter>(
                    serviceCenterNumberResult.Error);
            }

            var locationNameResult = LocationName.Create(
                source.LocationName);

            if (locationNameResult.IsFailure)
            {
                return Result.Failure<ServiceCenter>(locationNameResult.Error);
            }

            var roadNameResult = RoadName.Create(source.RoadName);

            if (roadNameResult.IsFailure)
            {
                return Result.Failure<ServiceCenter>(roadNameResult.Error);
            }

            var buildingNumberResult = BuildingNumber.Create(
                source.BuildingNumber);

            if (buildingNumberResult.IsFailure)
            {
                return Result.Failure<ServiceCenter>(buildingNumberResult.Error);
            }

            var addressResult = Address.Create(
                locationName: locationNameResult.Value,
                roadName: roadNameResult.Value,
                buildingNumber: buildingNumberResult.Value);

            if (addressResult.IsFailure)
            {
                return Result.Failure<ServiceCenter>(addressResult.Error);
            }

            var serviceCenterId = new ServiceCenterId(Guid.NewGuid());

            var serviceCenterResult = ServiceCenter.Create(
                serviceCenterId,
                address: addressResult.Value,
                number: serviceCenterNumberResult.Value);

            if (serviceCenterResult.IsFailure)
            {
                return Result.Failure<ServiceCenter>(serviceCenterResult.Error);
            }

            return serviceCenterResult.Value;
        }
    }
}
