using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.ServiceCenters.DTO;
using TraffiLearn.Domain.Aggregates.ServiceCenters;

namespace TraffiLearn.Application.ServiceCenters.Mappers
{
    internal sealed class ServiceCenterToResponseDtoMapper
        : Mapper<ServiceCenter, ServiceCenterResponse>
    {
        public override ServiceCenterResponse Map(ServiceCenter source)
        {
            return new ServiceCenterResponse(
                Id: source.Id.Value,
                ServiceCenterNumber: source.Number.Value,
                LocationName: source.Address.LocationName.Value,
                RoadName: source.Address.RoadName.Value,
                BuildingNumber: source.Address.BuildingNumber.Value);
        }
    }
}
