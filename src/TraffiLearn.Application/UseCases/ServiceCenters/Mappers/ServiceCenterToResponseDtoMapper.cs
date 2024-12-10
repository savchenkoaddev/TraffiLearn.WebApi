using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.UseCases.ServiceCenters.DTO;
using TraffiLearn.Domain.ServiceCenters;

namespace TraffiLearn.Application.UseCases.ServiceCenters.Mappers
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
